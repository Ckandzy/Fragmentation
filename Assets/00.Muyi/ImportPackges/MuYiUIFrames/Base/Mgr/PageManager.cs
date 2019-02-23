using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
    // 基于栈的Page管理，如果多个Page处于并行状态，且随时可以跳转的情况,则不能使用
	public class PageManager<T> where T : UIManager{
        protected Stack<UIType> pageStack = new Stack<UIType>();
        public PageManager() { }

        public void Push(UIType _type)
        {
            // if pagestack.count > 0, make peek onPause
            if(pageStack.Count != 0)
            {
                UIType pagePeek = pageStack.Peek();
                BaseView view = SingleTon<T>.Instance.GetByUIType(pagePeek).gameObject.GetComponent<AnimaView>();
                view.OnPause(pagePeek);
            }

            pageStack.Push(_type);
            GameObject pushObj = SingleTon<T>.Instance.GetPageGameObj(_type);
            BaseView pushView = pushObj.GetComponent<AnimaView>();
            pushView.OnEnter(_type);
        }

        public void Pop()
        {
            if(pageStack.Count > 0)
            {
                UIType popType = pageStack.Pop();
                GameObject popObj = SingleTon<T>.Instance.GetByUIType(popType);
                BaseView popview = popObj.GetComponent<AnimaView>();
                popview.OnExit(popType);
                // GameObject.Destroy(popObj);
            }
            
            if(pageStack.Count > 0)
            {
                UIType peek = pageStack.Peek();
                BaseView peekview = SingleTon<T>.Instance.GetByUIType(peek).GetComponent<AnimaView>();
                peekview.OnWake(peek);
            }
        }

        public UIType GetTopUIType()
        {
            if (pageStack.Count == 0) return null;
            return pageStack.Peek();
        }
	}
}


