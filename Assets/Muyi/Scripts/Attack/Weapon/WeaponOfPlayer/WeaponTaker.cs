using UnityEngine;
using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 合并到PlayerStatus中
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class WeaponTaker : MonoBehaviour
{
    // 武器
    public GameObject Hands; // 手
    public GameObject CurrentTakeWeaponSprite; // 当前携带的武器 -- 图片
    public Transform BulletPoint; // 子弹发射点
    public GameObject MeleeAttackDamager; // 近战武器范围框

    public int CurrentIndex;
    public IWeapon[] CurrentTakeWeapons = new IWeapon[4]; // 当前携带的武器


    // 碎片
    public IFragment[] TakeFragments = new IFragment[4]; // 

    protected readonly int m_HashRangeAttak = Animator.StringToHash("RangeAttack");
    protected readonly int m_HashIdle = Animator.StringToHash("Grounded");
    protected readonly int m_HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");

    private Animator m_Animator;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        TakeUpWeapon(WeaponType.RangeType, 1);
        SceneLinkedSMB<WeaponTaker>.Initialise(GetComponent<Animator>(), this);
    }

    private void Update()
    {
        //vec = new Vector2(GetComponent<SpriteRenderer>().flipX ? -1 : 1, 0);
        //if (Gamekit2D.PlayerInput.Instance.Horizontal.Value != 0 || Gamekit2D.PlayerInput.Instance.Vertical.Value != 0)
        //   vec = new Vector2(Gamekit2D.PlayerInput.Instance.Horizontal.Value, Gamekit2D.PlayerInput.Instance.Vertical.Value);
        if (CurrentTakeWeapons[CurrentIndex] != null) CurrentTakeWeapons[CurrentIndex].Update();
        CutoverWeaponByKeyCode();
        CheckWeaponCanTake();
    }

    private void FixedUpdate()
    {
        
    }

    #region 攻击
    // 进入动画，
    public void Attack()
    {
        if(CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.RangeType)
        {
            m_Animator.SetTrigger(m_HashRangeAttak);
        }
        else if(CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.MeleeType)
        {
            m_Animator.SetTrigger(m_HashMeleeAttackPara);
        }
    }
    // 释放子弹，调整碰撞框
    public void WeaponAttackEnter()
    {
        if(CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.RangeType)
            CurrentTakeWeapons[CurrentIndex].Attack(BulletPoint, new Vector2(transform.localScale.x, 0), new List<IBuff> { });
        else if (CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.MeleeType)
        {
            MeleeAttackDamager.SetActive(true);
        }
    }

    public void WeaponAttckOver()
    {
        if (CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.MeleeType)
        {
            MeleeAttackDamager.SetActive(false);
        }
    }

    #endregion

    #region 武器携带
    // 添加武器
    public void TakeUpWeapon(WeaponType type, int id)
    {
        for(int i = 0; i < CurrentTakeWeapons.Length; i++)
        {
            //Debug.Log(CurrentTakeWeapons[i] == null);
            if (CurrentTakeWeapons[i] == null)
            {
                CurrentTakeWeapons[i] = WeaponFactory.GetWeapon(type, id);
                CurrentTakeWeapons[i].Init();
                CutoverWeapon(i);
                break;
            }
        }
    }

    // 武器切换
    public void CutoverWeapon(int index)
    {
        if(CurrentTakeWeapons[index] != null)
        {
            if (CurrentIndex != index)
            {
                CurrentIndex = index;
                CurrentTakeWeaponSprite.GetComponent<SpriteRenderer>().sprite = CurrentTakeWeapons[index].sprite;
            }
            AdjustCurrentWeapon(CurrentTakeWeapons[index].getWeaponType(), index);
        }
    }

    // 远程与近战武器携带时的角度调整 -- 如果是远程就调整子弹生成点 -- 如果是近战，就调整伤害框
    private void AdjustCurrentWeapon(WeaponType type, int index)
    {
        switch (type)
        {
            case WeaponType.MeleeType:
                Hands.transform.localEulerAngles = new Vector3(0, 0, -80f);
                CurrentTakeWeaponSprite.transform.localEulerAngles = new Vector3(0, 0, 90f);
                MeleeAttackDamager.GetComponent<Damager>().offset = ((MeleeWeapon)CurrentTakeWeapons[index]).offset;
                MeleeAttackDamager.GetComponent<Damager>().size = ((MeleeWeapon)CurrentTakeWeapons[index]).size;
                break;
            case WeaponType.RangeType:
                Hands.transform.localEulerAngles = new Vector3(0, 0, -45f);
                CurrentTakeWeaponSprite.transform.localEulerAngles = new Vector3(0, 0, 0f);
                if (CurrentTakeWeapons[index].getWeaponType() == WeaponType.RangeType)
                    BulletPoint.localPosition = ((RangedWeapon)CurrentTakeWeapons[index]).OffsetPoint;
                break;
        }
    }

    // 按键1,2,3,4切换武器
    public void CutoverWeaponByKeyCode()
    {
        //if (PlayerInput.Instance.Weapon1.Down) { Debug.Log("1 down"); }
        if (PlayerInput.Instance.Weapon1.Down) CutoverWeapon(0);
        else if (PlayerInput.Instance.Weapon2.Down) CutoverWeapon(1);
        else if (PlayerInput.Instance.Weapon3.Down) CutoverWeapon(2);
        else if (PlayerInput.Instance.Weapon4.Down) CutoverWeapon(3);
    }


    // 卸下武器。
    public void TakeDownWeapon()
    {
        CurrentTakeWeaponSprite.GetComponent<SpriteRenderer>().sprite = null;
    }

    // 检查武器是否可以捡
    public void CheckWeaponCanTake()
    {
        Vector2 offset = transform.position;
        Vector2 size = new Vector2(-1.5f, +1.5f);

        Collider2D collider = Physics2D.OverlapArea(offset - size, offset + size, 1 << 11);

        Physics2D.queriesStartInColliders = false;

        //Debug.DrawLine(offset - size, offset + size);

        //Debug.Log(collider);

        if(collider != null && PlayerInput.Instance.Interact.Down)
        {
            //TakeUpWeapon(collider.GetComponent<WeaponId>().type, collider.GetComponent<WeaponId>().ID);
            //Destroy(collider.gameObject);
            PickUp(collider);
        }
    }
    #endregion

    public void PickUp(Collider2D collider)
    {
        //Debug.Log("拾取");
        switch (collider.tag)
        {
            case "CanTakeWeapon":
                TakeUpWeapon(collider.GetComponent<WeaponId>().type, collider.GetComponent<WeaponId>().ID);
                Destroy(collider.gameObject);
                break;
            case "Fragment":
                Fragments frag = collider.GetComponent<Fragments>();
                AddFragment(frag.type, frag.fragmentID, frag.sprite);
                Destroy(collider.gameObject);
                break;
        }
    }

    /// <summary>
    /// 添加碎片
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void AddFragment(FragmentType type, int id, Sprite _sprite)
    {
        // UI上显示
        FragmenMgr.Instance.AddFragmentItem(type, id, _sprite);
    }
}

