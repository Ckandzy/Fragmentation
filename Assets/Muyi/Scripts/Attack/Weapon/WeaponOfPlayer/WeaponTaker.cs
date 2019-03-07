using UnityEngine;
using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
/// <summary>
/// 合并到PlayerStatus中
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Status))]
public class WeaponTaker : MonoBehaviour
{
    [System.Serializable]
    public class FragEvent : UnityEvent<FragmentName, Sprite> { }

    // 武器
    public GameObject Hands; // 手
    public GameObject CurrentTakeWeaponSprite; // 当前携带的武器 -- 图片
    public Transform BulletPoint; // 子弹发射点
    public GameObject MeleeAttackDamager; // 近战武器范围框
    public Transform CenterPoint; // 中心
    public int CurrentIndex = -1;
    public IWeapon[] CurrentTakeWeapons = new IWeapon[4]; // 当前携带的武器
    public ABulletsPool BulletsPool;
    // 子弹池父类
    public GameObject PoolGameobj;
    // 碎片
    public IFragment[] TakeFragments = new IFragment[4]; // 
    public FragEvent OnFragAdd;
    public SkillBase CurrentSkill = null;

    protected readonly int m_HashRangeAttak = Animator.StringToHash("RangeAttack");
    protected readonly int m_HashIdle = Animator.StringToHash("Grounded");
    protected readonly int m_HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");

    public Audio PickUpWeaponAudio;
    public Audio SkillSource;
    public RangeWaponAttackAudio RangeWeaponAttaAudio;
    public MeleeAttakAudio MeleeAcAudio;

    private Animator m_Animator;
    private Status m_Status;

    private bool m_InBattle = false;
    private float battleTimer = 0f;
    private float m_OutBattleTime = 1.5f;

    private void Awake()
    {
        PoolGameobj = new GameObject("PoolGameobj");
        m_Animator = GetComponent<Animator>();
        SceneLinkedSMB<WeaponTaker>.Initialise(GetComponent<Animator>(), this);
        BulletsPool = GetComponent<ABulletsPool>();
        m_Status = GetComponent<Status>();
    }

    private void Start()
    {
        TakeUpWeapon(WeaponType.RangeType, 4);
    }

    private void Update()
    {
        //vec = new Vector2(GetComponent<SpriteRenderer>().flipX ? -1 : 1, 0);
        //if (Gamekit2D.PlayerInput.Instance.Horizontal.Value != 0 || Gamekit2D.PlayerInput.Instance.Vertical.Value != 0)
        //   vec = new Vector2(Gamekit2D.PlayerInput.Instance.Horizontal.Value, Gamekit2D.PlayerInput.Instance.Vertical.Value);
        if (CurrentTakeWeapons[CurrentIndex] != null) CurrentTakeWeapons[CurrentIndex].Update();
        CutoverWeaponByKeyCode();
        CheckWeaponCanTake();

        if (CurrentSkill != null) CurrentSkill.SkillUpdate();

        if (m_InBattle && CurrentTakeWeapons[CurrentIndex] != null && CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.RangeType)
        {
            InBattle();
            battleTimer += Time.deltaTime;
            if (battleTimer >= m_OutBattleTime)
                OutBattle();
        }
    }

    private void InBattle()
    {
        m_InBattle = true;
        Hands.transform.localEulerAngles = new Vector3(0, 0, 0f);
    }

    private void OutBattle()
    {
        m_InBattle = false;
        Hands.transform.localEulerAngles = new Vector3(0, 0, -45f);
    }

    private void FixedUpdate()
    {
        
    }

    #region 技能

    public void UseSkill()
    {
        if (CurrentSkill != null && CurrentSkill.MSkillStatus == SkillStatusEnum.Ready)
        {
            CurrentSkill.UseSkill(CenterPoint);
            SkillSource.PlayRandomSound((int)CurrentSkill.SkillName());
            FragmenMgr.Instance.SlotCanInteraciveNotify(false);
        }
    }

    public void SkillOver()
    {
        SkillSource.Stop();
    }

    public void SetSkill(SkillBase skill)
    {
        CurrentSkill = skill;
        if (skill != null)
        {
            CurrentSkill.SkillEnter();
        }
        
        SkillSlot.Instance.SkillInit(CurrentSkill);
    }
    #endregion

    #region 近战音效
    public void HitAir(TakeDamager take)
    {
        MeleeAcAudio.PlayRandomSound(HitObject.Air);
    }

    public void HitEnemy(TakeDamager take, TakeDamageable takeDamageable)
    {
        MeleeAcAudio.Stop();
        MeleeAcAudio.PlayRandomSound(HitObject.Enemy);
    }
    #endregion

    #region 攻击
    // 进入动画，
    public void Attack()
    {
        if(CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.RangeType)
        {
            InBattle();
            m_Animator.SetTrigger(m_HashRangeAttak);
            battleTimer = 0;
           
        }
        else if(CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.MeleeType)
        {
            m_Animator.SetTrigger(m_HashMeleeAttackPara);
        }
    }

    // 远程释放子弹，近战调整碰撞框
    public void WeaponAttackEnter()
    {
        if(CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.RangeType )
        {
            // 特殊情况---改   乱得一匹
            if (CurrentTakeWeapons[CurrentIndex].GetWeaponName() == WeaponName.SubmachineGun )
            {
                StartCoroutine(((SubmachineGun)CurrentTakeWeapons[CurrentIndex]).Shoot(m_Status, BulletsPool, BulletPoint, new Vector2(transform.localScale.x, 0), new List<IBuff> { }));
            } 
            else if (CurrentTakeWeapons[CurrentIndex].GetWeaponName() == WeaponName.Shotgun)
            {
                StartCoroutine(((Shotgun)CurrentTakeWeapons[CurrentIndex]).Shoot(m_Status, BulletsPool, BulletPoint, new Vector2(transform.localScale.x, 0), new List<IBuff> { }));
            }
            else
            {
                GameObject bullet = BulletsPool.Pop().transform.gameObject;
                m_Status.RegisteredTakeDamger(bullet.GetComponent<TakeDamager>());
                ((RangedWeapon)CurrentTakeWeapons[CurrentIndex]).Attack(bullet, BulletPoint, new Vector2(transform.localScale.x, 0), new List<IBuff> { });
            }
            RangeWeaponAttaAudio.PlayRandomSound(CurrentTakeWeapons[CurrentIndex].GetWeaponName()); 
        }
        else if (CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.MeleeType)
        {
            MeleeAttackDamager.SetActive(true);
            HitAir(null);
        }
    }

    public void WeaponAttckOver()
    {
        if (CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.MeleeType)
        {
            MeleeAttackDamager.SetActive(false);
        }
        else if (CurrentTakeWeapons[CurrentIndex].getWeaponType() == WeaponType.RangeType)
        {
            InBattle();
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
                PickUpWeaponAudio.PlayRandomSound((int)CurrentTakeWeapons[index].getWeaponType());
                CurrentIndex = index;
            }
            CurrentTakeWeaponSprite.GetComponent<SpriteRenderer>().sprite = CurrentTakeWeapons[index].sprite;
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
                TakeDamager takeDamager = MeleeAttackDamager.GetComponent<TakeDamager>();
                takeDamager.offset = ((MeleeWeapon)CurrentTakeWeapons[index]).offset;
                takeDamager.size = ((MeleeWeapon)CurrentTakeWeapons[index]).size;
                takeDamager.hitPoint = ((MeleeWeapon)CurrentTakeWeapons[index]).HitPoint;
                break;
            case WeaponType.RangeType:
                Hands.transform.localEulerAngles = new Vector3(0, 0, -45f);
                CurrentTakeWeaponSprite.transform.localEulerAngles = new Vector3(0, 0, 0f);
                //if (CurrentTakeWeapons[index].getWeaponType() == WeaponType.RangeType)
                BulletPoint.localPosition = ((RangedWeapon)CurrentTakeWeapons[index]).OffsetPoint;
                // resert bullet pool
                GameObject bullet = ((RangedWeapon)CurrentTakeWeapons[index]).bullet;
                BulletsPool.prefab = bullet;
                GameObject[] bullets = BulletsPool.Restart(PoolGameobj.transform, 5);
                foreach (GameObject obj in bullets) {m_Status.RegisteredTakeDamger(obj.GetComponent<TakeDamager>()); }
                m_Status.TakeDamagerFlush();
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
                AddFragment(frag.FragName, frag.sprite);
                Destroy(collider.gameObject);
                break;
        }
    }

    /// <summary>
    /// 添加碎片
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void AddFragment(FragmentName _name, Sprite _sprite = null)
    {
        // UI上显示
        //FragmenMgr.Instance.AddFragmentItem(_name, _sprite);
        OnFragAdd.Invoke(_name, _sprite);
    }
}

