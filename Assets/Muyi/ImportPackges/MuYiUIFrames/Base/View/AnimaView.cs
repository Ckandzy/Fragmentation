using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
    [RequireComponent(typeof(Animator))]
	public class AnimaView : BaseView {
        protected Animator animator;

        public readonly int onEnter = Animator.StringToHash("OnEnter");
        public readonly int onExit = Animator.StringToHash("OnExit") ;
        public readonly int onPause = Animator.StringToHash("OnPause");
        public readonly int onWake = Animator.StringToHash("OnWake");

        public override void OnEnter(UIType _type)
        {
            if (animator == null) animator = GetComponent<Animator>();
            animator.SetBool(onEnter, true);
            animator.SetBool(onExit, false);
        }

        public override void OnExit(UIType _type)
        {
            base.OnExit(_type);
            animator.SetBool(onEnter, false);
            animator.SetBool(onExit, true);
        }

        public override void OnPause(UIType _type)
        {
            base.OnPause(_type);
            animator.SetBool(onPause, true);
            animator.SetBool(onWake, false);
        }

        public override void OnWake(UIType _type)
        {
            base.OnWake(_type);
            animator.SetBool(onPause, false);
            animator.SetBool(onWake, true);
        }
    }
}


