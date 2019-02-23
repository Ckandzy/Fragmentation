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
    // 当前碎片的信息
    Dictionary<FragmentType, int> FragDic;
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
    /// 返回已经装备的碎片的所有Id和type
    /// </summary>
    /// <returns></returns>
    public Dictionary<FragmentType, int> GetEquipFragAttr()
    {
        Dictionary<FragmentType, int> dic = new Dictionary<FragmentType, int>();
        IFragment frag;
        foreach (FragmentSlot slot in EquipSlot)
        {
            if (slot.ItemChild != null)
            {
                frag = slot.ItemChild.GetComponent<FragmentItem>().ItemFragment;
                dic.Add(frag.GetFragType(), frag.ID);
            }
            
        }
        return dic;
    }
    
    public void ShowInfo(FragmentItem _item)
    {
        if (_item == null) InfoText.text = "当前格子无碎片";
        else InfoText.text = _item.Des();
    }
}


