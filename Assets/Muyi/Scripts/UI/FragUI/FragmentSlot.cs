using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class FragmentSlot : MSlot, IPointerClickHandler {
    [System.Serializable]
    public class SlotClickEvent : UnityEvent<FragmentItem>{ }
    public SlotClickEvent OnSlotClick;

    private new void Add(int _id, GameObject ItemPrefab)
    {
        //base.Add(_id, ItemPrefab);
    }

    public void AddFragmentItem(FragmentName _name, GameObject ItemPrefab, Sprite _sprite)
    {
        ItemChild = Instantiate(ItemPrefab, transform).transform;
        ItemChild.GetComponent<Image>().sprite = _sprite;
        ItemChild.GetComponent<FragmentItem>().ItemFragment = FragmentFactory.GetFragment(_name);
        isEmpty = false;
        ItemChild.localPosition = Vector3.zero;
        OnItemAdd.Invoke(this, ItemChild.GetComponent<FragmentItem>());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("点击");
        if (ItemChild == null) OnSlotClick.Invoke(null);
        else OnSlotClick.Invoke(ItemChild.GetComponent<FragmentItem>());
    }

   
}


