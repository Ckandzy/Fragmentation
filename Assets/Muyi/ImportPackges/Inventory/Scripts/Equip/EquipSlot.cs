using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    public class EquipSlot : MonoBehaviour
    {
        public int ItemID = -1;
        public Transform Item;
        public void Add(int _id, GameObject _item)
        {
            ItemID = _id;
            Item = Instantiate(_item).transform;
            Item.SetParent(transform);
            Destroy(Item.GetComponent<InvItemBase>());
            Item.localPosition = Vector2.zero;
        }
    }

    public class EquipItem : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                Inventory.instance.Add(transform.parent.GetComponent<EquipSlot>().ItemID);
            }
        }
    }
}


