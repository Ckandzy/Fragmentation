using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;

namespace Inv
{

	public class IntroPanelAnima : NotifiedAnima {
        public override void Notify(UIType _type)
        {
            OnExit(_type);
        }

        public override void OnEnter(UIType _type)
        {
            base.OnEnter(_type);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }

        public override void OnExit(UIType _type)
        {
            base.OnEnter(_type);
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
        }

        public override void OnPause(UIType _type)
        {
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
        }

        public override void OnWake(UIType _type)
        {
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }
    }
}


