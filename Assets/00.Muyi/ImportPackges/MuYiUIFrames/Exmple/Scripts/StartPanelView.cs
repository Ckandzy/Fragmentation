using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
	public class StartPanelView : AnimaView {

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void OnEnter(UIType _type)
        {
            Debug.Log(animator);
            animator = GetComponent<Animator>();
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

        public void ShowTesk()
        {
            SingleTon<PageManager<UIManager>>.Instance.Push(GameTest.teskpanel);
        }
    }
}


