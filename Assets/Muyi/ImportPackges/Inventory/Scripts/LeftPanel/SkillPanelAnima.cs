using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;

namespace Inv
{
    public class SkillPanelAnima : NotifiedAnima
    {
        public override void OnEnter(UIType _type)
        {
            base.OnEnter(_type);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }
        public override void OnExit(UIType _type)
        {
            base.OnExit(_type);
            base.OnEnter(_type);
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
        }
        public override void OnPause(UIType _type)
        {
            base.OnPause(_type);
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
        }

        public override void OnWake(UIType _type)
        {
            base.OnWake(_type);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }

        public override void Notify(UIType uIType)
        {
            OnExit(uIType);
        }
    }
}


