using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem
{
    public class InvItemBase : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected Transform canvas;
        public int ID;
        public bool isFindSlot = false;
        public InvSlotBase nowSlot;

        protected virtual void Start()
        {
            canvas = GameObject.Find("Canvas").transform;
            nowSlot = transform.parent.GetComponent<InvSlotBase>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            nowSlot = transform.parent.GetComponent<InvSlotBase>();
            if (transform != null)
            {
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                isFindSlot = false;
                //rect.position = eventData.position;
                transform.SetParent(canvas);
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            RectTransform rect = GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.position = eventData.position;
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (isFindSlot) return;
            RectTransform rect = GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.SetParent(nowSlot.transform);
                rect.localPosition = Vector2.zero;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}


