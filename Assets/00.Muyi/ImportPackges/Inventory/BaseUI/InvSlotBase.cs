using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UISystem
{
    public class InvSlotBase : MonoBehaviour, IDropHandler
    {
        public bool isEmpty = true;
        public int ItemID;
        public Transform ItemChild;
        public int SlotIndex;
        protected int itemNum = 0;
        public int ItemNum
        {
            get { return itemNum; }
            set
            {
                itemNum = value;
                if (itemNum == 0)
                {
                    if (ItemChild != null) Destroy(ItemChild.gameObject);
                    isEmpty = true;
                }
                if (itemNum <= 1)
                    ItemNum_Text.gameObject.SetActive(false);
                else
                {
                    ItemNum_Text.gameObject.SetActive(true);
                    ItemNum_Text.text = itemNum.ToString();
                }
            }
        }
        protected Text ItemNum_Text;

        private void Awake()
        {
            ItemNum_Text = transform.GetComponentInChildren<Text>();
            ItemNum = 0;
        }

        /// <summary>
        /// Empty the slot
        /// </summary>
        public virtual void Empty()
        {
            ItemNum = 0;
            if (ItemChild != null) Destroy(ItemChild.gameObject);
        }

        /// <summary>
        /// Add Item
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_itemChild"></param>
        public virtual void Add(int _id, GameObject ItemPrefab, int addNum = 1)
        {

        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            
        }

        protected virtual void Exchange(InventoryItem _item)
        {
            if (isEmpty)
            {
                // 赋值slot信息
                // ItemID, itemChild, ItemNum
                InvSlotBase slot = _item.nowSlot.GetComponent<InvSlotBase>();
                ItemID = slot.ItemID;
                ItemChild = slot.ItemChild;
                ItemNum = slot.ItemNum;
                //ItemData date = slot.iten
                // 清除该item父slot的信息
                slot.ItemID = -1;
                slot.ItemChild = null;
                slot.ItemNum = 0;
                // 交换位置改写item信息
                _item.nowSlot.isEmpty = true;
                _item.transform.parent = transform;
                _item.transform.SetSiblingIndex(0);
                _item.transform.localPosition = Vector2.zero;
                _item.isFindSlot = true;
                _item.nowSlot = this;

                ItemNum += 1;
            }
            else
            {
                // 交换 
                // ItemID, itemChild, ItemNum
                // 1 保存pointerDrag的当前格子三个信息
                InvSlotBase pointerSlot = _item.nowSlot.GetComponent<InvSlotBase>();
                int Pid = pointerSlot.ItemID;
                Transform Pitemchild = pointerSlot.ItemChild;
                int Pitmenum = pointerSlot.ItemNum;
                // 防止执行item的itemDragEnter函数
                _item.isFindSlot = true;
                // 2 交换item
                // 将当前slot的item交给PointerItem的slot -信息
                pointerSlot.ItemID = ItemID;
                pointerSlot.ItemChild = ItemChild;
                pointerSlot.ItemNum = ItemNum;
                // item位置
                ItemChild.transform.SetParent(pointerSlot.transform);
                ItemChild.transform.SetSiblingIndex(0);
                ItemChild.transform.localPosition = Vector2.zero;
                // item.nowSlot
                ItemChild.GetComponent<InvItemBase>().nowSlot = this;
                Debug.Log(pointerSlot.transform.GetSiblingIndex());
                // 3 将拖拽的item赋值给当前slot
                // 信息
                ItemID = Pid;
                ItemChild = Pitemchild;
                ItemNum = Pitmenum;

                // item 位置
                _item.transform.SetParent(transform);
                _item.transform.SetSiblingIndex(0);
                _item.transform.localPosition = Vector2.zero;
                //item.nowSLot
                _item.nowSlot = this;
            }
        }
    }
}


