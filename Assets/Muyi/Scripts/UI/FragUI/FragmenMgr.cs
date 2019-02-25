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
    public List<int> EquipBuffIds = new List<int>();

    private void Start()
    {
        foreach (MSlot mSlot in EquipSlot) mSlot.OnExcange = FlushEquipFragmentBuffIds;
        ExtraSlot.OnExcange = FlushEquipFragmentBuffIds;
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
        // 更新信息
       // FragDic = GetEquipFragAttr();
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

    public void FlushEquipFragmentBuffIds()
    {
        EquipBuffIds = new List<int>();
        foreach(FragmentSlot slot in EquipSlot)
        {
            IFragment frag = slot.ItemChild.GetComponent<IFragment>();
            if (frag != null)
            {
                EquipBuffIds.AddListInt(frag.GetBuffIDs());
            }
        }
    }
}