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
    //public List<int> EquipBuffIds = new List<int>();

    private Status m_PlayerStatus;

    private void Awake()
    {
        m_PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();
    }

    private void Start()
    {
        foreach (MSlot mSlot in EquipSlot)
        {
            mSlot.OnItemAdd.AddListener(this.AddFragment);
            mSlot.OnItemRemove.AddListener(this.RemoveFragment);
        }
    }

    // 添加碎片
    public void AddFragmentItem(FragmentType type, int itemid, Sprite _sprite)
    {
        bool hasEmpty = false;
        foreach(FragmentSlot mSlot in EquipSlot)
        {
            if (mSlot.isEmpty)
            {
                mSlot.AddFragmentItem(type, itemid, ItemPrefab, _sprite);
                hasEmpty = true;
                break;
            }
        }
        if (!hasEmpty)
        {
            ExtraSlot.AddFragmentItem(type, itemid, ItemPrefab, _sprite);
        }
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
    public void RemoveFragment(MSlot slot, MItem item)
    {
        foreach (IBuff buff in ((FragmentItem)item).ItemFragment.buffs)
        {
            m_PlayerStatus.RemoveStatuBuff(buff);
            Debug.Log("Remove");
        }
    }

    public void AddFragment(MSlot slot, MItem item)
    {
        foreach (IBuff buff in ((FragmentItem)item).ItemFragment.buffs)
        {
            m_PlayerStatus.AddStatusBuff(buff);
        }
    }
    #endregion
}