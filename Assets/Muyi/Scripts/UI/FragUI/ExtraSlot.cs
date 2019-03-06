using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ExtraSlot : MSlot, IPointerClickHandler
{
    [System.Serializable]
    public class SlotClickEvent : UnityEvent<FragmentItem> { }
    public SlotClickEvent OnSlotClick;

    public sealed override void Add(int _id, GameObject ItemPrefab)
    {
        base.Add(_id, ItemPrefab);
    }

    public void AddFragmentItem(FragmentName _name, GameObject ItemPrefab, Sprite _sprite)
    {
        if(ItemChild == null)
        {
            ItemChild = Instantiate(ItemPrefab, transform).transform;
        }
        ItemChild.GetComponent<Image>().sprite = _sprite;
        ItemChild.GetComponent<FragmentItem>().ItemFragment = FragmentFactory.GetFragment(_name);
        isEmpty = false;
        ItemChild.localPosition = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemChild == null) OnSlotClick.Invoke(null);
        else OnSlotClick.Invoke(ItemChild.GetComponent<FragmentItem>());
    }
}


