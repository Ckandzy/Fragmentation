using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem
{
    public class InventoryItem : InvItemBase
    {
        private ObjectType type;

        protected override void Start()
        {
            //Debug.Log(ID);
            base.Start();
            type = WeaponAL.GetWeaponType(AllObjectInfos.instance.GetObject(ID).Type);
        }
        public void Update()
        {
            Debug.Log(type);
            if(type == ObjectType.Weapon)
                if (Input.GetMouseButton(1))
                {
                    // 装备物体
                    // 后面改为菜单组件
                    EquipInv.instance.Add(ID, Inventory.instance.Item.gameObject);
                    //Inventory.instance.RemoveItem(ID);
                    //nowSlot.ItemNum -= 1;
                }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            nowSlot = transform.parent.GetComponent<InvSlotBase>();
            if (rect != null)
            {
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                isFindSlot = false;
                //rect.position = eventData.position;
                rect.SetParent(canvas.transform);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.position = eventData.position;
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (isFindSlot) return;
            RectTransform rect = GetComponent<RectTransform>();
            if(rect != null)
            {
                rect.SetParent(nowSlot.transform);
                rect.localPosition = Vector2.zero;
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            Inventory.instance.ShowDes(ID, transform.position);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Inventory.instance.ShowDes(ID, transform.position, false);
        }
    }
}


