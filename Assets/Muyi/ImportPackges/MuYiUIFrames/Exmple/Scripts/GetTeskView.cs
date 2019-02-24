using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
	public class GetTeskView : AnimaView {
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

        public void Back()
        {
            SingleTon<PageManager<UIManager>>.Instance.Pop();
        }
    }
}


