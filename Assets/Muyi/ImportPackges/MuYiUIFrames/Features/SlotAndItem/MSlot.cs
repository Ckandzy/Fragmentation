using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace MuyiFrame
{
    public class MSlot : MonoBehaviour, IDropHandler
    {
        [System.Serializable]
        public class OnItemRemoveEvent : UnityEvent<MSlot, MItem> { }
        [System.Serializable]
        public class OnItemAddEvent : UnityEvent<MSlot, MItem> { }

        public bool isEmpty = true;
        public Transform ItemChild;
        public int ItemID;
        public OnItemAddEvent OnItemAdd;
        public OnItemRemoveEvent OnItemRemove;

        public bool CanInteractive = true;
        private void Awake()
        {

        }
        public System.Action OnExcange; 

        /// <summary>
        /// Empty the slot
        /// </summary>
        public virtual void Empty()
        {
            if (ItemChild != null)
            {
                OnItemRemove.Invoke(this, ItemChild.GetComponent<MItem>());
                Destroy(ItemChild.gameObject);
            }
        }

        /// <summary>
        /// Add Item
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_itemChild"></param>
        public virtual void Add(int _id, GameObject ItemPrefab)
        {
            OnItemAdd.Invoke(this, ItemPrefab.GetComponent<MItem>());
        }

        public void OnDrop(PointerEventData eventData)
        {
            MItem item = eventData.pointerDrag.GetComponent<MItem>();
            if (item == null) return;
            Exchange(item);
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_item">指向的item</param>
        protected virtual void Exchange1(MItem _item)
        {
            //MSlot slot = _item.nowSlot.GetComponent<MSlot>();
            if (isEmpty)
            {
                // 赋值slot信息
                // ItemID, itemChild, ItemNum
                MSlot slot = _item.nowSlot.GetComponent<MSlot>();
                ItemID = slot.ItemID;
                ItemChild = slot.ItemChild;

                //ItemData date = slot.iten
                // 清除该item父slot的信息
                slot.ItemID = -1;
                slot.ItemChild = null;
                slot.isEmpty = true;
                // 交换位置改写item信息
                _item.transform.parent = transform;
                //_item.transform.SetSiblingIndex(0);
                _item.transform.localPosition = Vector2.zero;
                _item.isFindSlot = true;
                _item.nowSlot = this;
                _item.nowSlot.isEmpty = false;
            }
            else
            {
                // 交换 
                // ItemID, itemChild, ItemNum
                // 1 保存pointerDrag的当前格子三个信息
                MSlot pointerSlot = _item.nowSlot.GetComponent<MSlot>();
                int Pid = pointerSlot.ItemID;
                Transform Pitemchild = pointerSlot.ItemChild;
                // 防止执行item的itemDragEnter函数
                _item.isFindSlot = true;
                // 2 交换item
                // 将当前slot的item交给PointerItem的slot -信息
                pointerSlot.ItemID = ItemID;
                pointerSlot.ItemChild = ItemChild;
                pointerSlot.isEmpty = false;
                // item位置
                ItemChild.transform.SetParent(pointerSlot.transform);
                //ItemChild.transform.SetSiblingIndex(0);
                ItemChild.transform.localPosition = Vector2.zero;
                //isEmpty = false;
                // item.nowSlot
                ItemChild.GetComponent<MItem>().nowSlot = this;
                Debug.Log(pointerSlot.transform.GetSiblingIndex());
                // 3 将拖拽的item赋值给当前slot
                // 信息
                ItemID = Pid;
                ItemChild = Pitemchild;
                // item 位置
                _item.transform.SetParent(transform);
                _item.transform.SetSiblingIndex(0);
                _item.transform.localPosition = Vector2.zero;
                //item.nowSLot
                _item.nowSlot = this;
                _item.nowSlot.GetComponent<MSlot>().isEmpty = false;
            }

            OnExcange();
        }
        */

        public void Exchange(MItem _item)
        {
            if (!CanInteractive) return;
            if (isEmpty)
            {
                // 赋值slot信息
                // ItemID, itemChild, ItemNum
                MSlot slot = _item.nowSlot;
                ItemID = slot.ItemID;
                ItemChild = slot.ItemChild;

                // 事件
                slot.OnItemRemove.Invoke(slot, _item);
                OnItemAdd.Invoke(this, _item);
                

                // 清除该item父slot的信息
                slot.ItemID = -1;
                slot.ItemChild = null;
                //slot.ItemNum = 0;
                slot.isEmpty = true;
                // 交换位置改写item信息
               
                _item.transform.parent = transform;
                _item.transform.localPosition = Vector2.zero;
                _item.nowSlot = this;
                _item.isFindSlot = true;
                isEmpty = false;
                
            }
            else
            {
                // 交换 
                // ItemID, itemChild, ItemNum
                // 1 保存pointerDrag的当前格子三个信息
                MSlot pointerSlot = _item.nowSlot.GetComponent<MSlot>();
                int Pid = pointerSlot.ItemID;
                Transform Pitemchild = pointerSlot.ItemChild;
                
                // 事件
                pointerSlot.OnItemRemove.Invoke(pointerSlot, pointerSlot.ItemChild.GetComponent<MItem>());
                pointerSlot.OnItemAdd.Invoke(pointerSlot, ItemChild.GetComponent<MItem>());
                OnItemRemove.Invoke(this, ItemChild.GetComponent<MItem>());
                OnItemAdd.Invoke(this, pointerSlot.ItemChild.GetComponent<MItem>());
                

                // 防止执行item的itemDragEnter函数
                _item.isFindSlot = true;
                // 2 交换item
                // 将当前slot的item交给PointerItem的slot -信息
                pointerSlot.ItemID = ItemID;
                pointerSlot.ItemChild = ItemChild;
                pointerSlot.isEmpty = false;
                
                // item位置
                ItemChild.transform.SetParent(pointerSlot.transform);
                ItemChild.transform.SetSiblingIndex(0);
                ItemChild.transform.localPosition = Vector2.zero;
                // item.nowSlot
                ItemChild.GetComponent<MItem>().nowSlot = this;
                //Debug.Log(pointerSlot.transform.GetSiblingIndex());
                // 3 将拖拽的item赋值给当前slot
                // 信息
                ItemID = Pid;
                ItemChild = Pitemchild;
                //ItemNum = Pitmenum;
                isEmpty = false;

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



