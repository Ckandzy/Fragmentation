using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class MeleeAttackBehavoir : SceneLinkedSMB<WeaponTaker> {

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.WeaponAttackEnter();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.MeleeAttackUpdate();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.MeleeAttackOver();
        m_MonoBehaviour.WeaponAttckOver();
    }
}


