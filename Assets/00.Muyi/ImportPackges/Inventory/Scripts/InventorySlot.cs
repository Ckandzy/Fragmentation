using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace UISystem
{
    public class InventorySlot : InvSlotBase
    {
        private void Awake()
        {
            ItemNum_Text = transform.GetComponentInChildren<Text>();
            ItemNum = 0;
        }

        /// <summary>
        /// Empty the slot
        /// </summary>
        public override void Empty()
        {
            ItemNum = 0;
            if (ItemChild != null) Destroy(ItemChild.gameObject);
        }

        /// <summary>
        /// Add Item
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_itemChild"></param>
        public override void Add(int _id, GameObject ItemPrefab, int addNum = 1)
        {
            if (isEmpty)
            {
                InventoryItem item = Instantiate(ItemPrefab, transform).GetComponent<InventoryItem>();
                item.ID = _id;
                item.GetComponent<RectTransform>().localPosition = Vector2.zero;
                string spriteName = AllObjectInfos.instance.GetObject(_id).SpriteName;
                Sprite sprite = Resources.Load<Sprite>(spriteName);
                item.transform.GetComponent<Image>().sprite = sprite;
                item.transform.SetSiblingIndex(0);
                item.nowSlot = this;

                isEmpty = false;
                ItemID = _id;
                ItemNum = addNum;
                ItemChild = item.transform;
            }
            else
            {
                ItemNum += addNum;
            }
        }

        public override void OnDrop(PointerEventData eventData)
        {
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (item == null) return;
            Exchange(item);
        }

        protected override void Exchange(InventoryItem _item)
        {
            base.Exchange(_item);
            Inventory.instance.UpdateDic(_item.ID, SlotIndex);
        }
    }
}


