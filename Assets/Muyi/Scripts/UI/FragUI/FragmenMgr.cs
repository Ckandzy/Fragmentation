using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using UnityEngine.UI;
public class FragmenMgr : MonoBehaviour {
    private static FragmenMgr _instance;
    public static FragmenMgr Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<FragmenMgr>();
            return _instance;
        }
    }
    // 拖拽
    public FragmentSlot[] EquipSlot;
    public ExtraSlot ExtraSlot;
    // item的gameobject
    public GameObject ItemPrefab;
    public Text InfoText;
    // 当前碎片携带的信息
    public StatisticsFrag Frags = new StatisticsFrag();

    private Status m_PlayerStatus;
    private WeaponTaker m_WeaponTaker;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerStatus = player.GetComponent<Status>();
        m_WeaponTaker = player.GetComponent<WeaponTaker>();
    }

    private void Start()
    {
        foreach (MSlot mSlot in EquipSlot)
        {
            mSlot.OnItemAdd.AddListener(this.AddFragment);
            mSlot.OnItemRemove.AddListener(this.RemoveFragment);
        }
    }

    public void NotifyWeaponTaker(SkillBase _skill)
    {
        m_WeaponTaker.SetSkill(_skill);
    }

    public void SlotCanInteraciveNotify(bool _isReady)
    {
        foreach (MSlot slot in EquipSlot)
        {
            slot.CanInteractive = _isReady;
        }
        ExtraSlot.CanInteractive = _isReady;
    }

    // 添加碎片
    public void AddFragmentItem(FragmentName _name, Sprite _sprite)
    {
        bool hasEmpty = false;
        foreach(FragmentSlot mSlot in EquipSlot)
        {
            if (mSlot.isEmpty)
            {
                mSlot.AddFragmentItem(_name, ItemPrefab, _sprite);
                hasEmpty = true;
                FlushSkill();
                break;
            }
        }
        if (!hasEmpty)
        {
            ExtraSlot.AddFragmentItem(_name, ItemPrefab, _sprite);
        }
    }
    
    public void FlushSkill()
    {
        Frags.FlushFrag(EquipSlot);
        skill = GetSkill();
        if (skill != null)
        {
            skill.OnSkillCDOver = SlotCanInteraciveNotify;
        }
        NotifyWeaponTaker(skill);
    }

    /// <summary>
    /// using event bind in onClick slot
    /// </summary>
    /// <param name="_item"></param>
    public void ShowInfo(FragmentItem _item)
    {
        if (_item == null) InfoText.text = "当前格子无碎片";
        else InfoText.text = _item.Des();
    }

    #region event of equipslot add or remove fragmentations
    SkillBase skill;
    public void RemoveFragment(MSlot slot, MItem item)
    {
        foreach (IBuff buff in ((FragmentItem)item).ItemFragment.buffs)
        {
            m_PlayerStatus.RemoveStatuBuff(buff);
        }
        FlushSkill();
    }

    public void AddFragment(MSlot slot, MItem item)
    {
        foreach (IBuff buff in ((FragmentItem)item).ItemFragment.buffs)
        {
            m_PlayerStatus.AddStatusBuff(buff);
        }
        FlushSkill();
    }
    #endregion

    public SkillBase GetSkill()
    {
        return Frags.GetSkill();
    }
}

// Statistics : 统计
public class StatisticsFrag
{
    public StatisticsFrag() { Init(); }

    public List<FragmentName> names = new List<FragmentName>(3) { FragmentName.Null, FragmentName.Null, FragmentName.Null };

    public void FlushFrag(FragmentSlot[] fragmentSlot)
    {
        for (int i = 0; i < fragmentSlot.Length; i++)
        {
            if (fragmentSlot[i].ItemChild != null && fragmentSlot[i].ItemChild.GetComponent<FragmentItem>())
            {
                names[i] = fragmentSlot[i].ItemChild.GetComponent<FragmentItem>().ItemFragment.GetFragName();
            }
            else
            {
                names[i] = FragmentName.Null;
            }
        }
    }

    public SkillBase GetSkill()
    {
        /*
        for (int i = 0; i < names.Count; i++)
        {
            if (names[i] == FragmentName.Null) return null;
        }*/
        for (int i = 0; i < fragmentsList.Count; i++)
        {
            if (isMatchSkill(fragmentsList[i]))
            {
                //if (i == 0) Debug.Log("获得技能");
                switch (i)
                {
                    case 0: return SKillFactory.GetSkill(1); // 无法无天
                    case 1: return SKillFactory.GetSkill(2); // 铁腕强权
                    case 2: return SKillFactory.GetSkill(3); // 风助火威
                }
            }
        }
        return null;
    }
    public List<SkillFragAttr> fragmentsList = new List<SkillFragAttr>();

    public void Init()
    {
        fragmentsList.Add(new SkillFragAttr(FragmentName.Disorder, FragmentName.Riot)); // 无法无天
        fragmentsList.Add(new SkillFragAttr(FragmentName.CriterionFrag, FragmentName.TrialFrag));
        fragmentsList.Add(new SkillFragAttr(FragmentName.FireFrag, FragmentName.WindFrag));
    }


    public bool isMatchSkill(SkillFragAttr attr)
    {
        for (int i = 0; i < attr.names.Length; i++)
        {
            if (!names.Contains(attr.names[i])) return false;
        }
        return true;
    }

    public class SkillFragAttr
    {
        public FragmentName[] names = new FragmentName[2];

        public SkillFragAttr(params FragmentName[] _names)
        {
            names = _names;
        }
    }
}