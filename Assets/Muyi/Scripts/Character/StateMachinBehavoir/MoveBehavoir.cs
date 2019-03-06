using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gamekit2D;
public class MoveBehavoir :  SceneLinkedSMB<WeaponTaker>{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnSLStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerInput.Instance.Attack.Down)
        {
            m_MonoBehaviour.Attack();
        }
        if (PlayerInput.Instance.UseSkill.Down)
        {
            m_MonoBehaviour.UseSkill();
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnSLStateExit(animator, stateInfo, layerIndex);
    }
}


