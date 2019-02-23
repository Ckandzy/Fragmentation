using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MuyiFrame
{
	public class TeskPanelView : AnimaView {
        public override void OnEnter(UIType _type)
        {
            base.OnEnter(_type);
            animator.SetBool("Exit", false);
            animator.SetBool("Enter", true);
        }

        public override void OnExit(UIType _type)
        {
            animator.SetBool("Exit", true);
            animator.SetBool("Enter", false);
        }

        public override void OnPause(UIType _type)
        {
            animator.SetBool("Exit", true);
            animator.SetBool("Enter", false);
        }

        public override void OnWake(UIType _type)
        {
            animator.SetBool("Exit", false);
            animator.SetBool("Enter", true);
        }

        public void GetTesk()
        {
            SingleTon<PageManager<UIManager>>.Instance.Push(GameTest.getteskpanel);

            transform.GetComponentInChildren<Text>().text = "you has got this tesk";
            transform.Find("Button (1)").gameObject.SetActive(false);
        }

        public void Back()
        {
            SingleTon<PageManager<UIManager>>.Instance.Pop();
        }
    }
}


