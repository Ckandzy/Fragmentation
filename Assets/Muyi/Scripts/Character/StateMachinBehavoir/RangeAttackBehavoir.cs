using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
public class RangeAttackBehavoir : SceneLinkedSMB<WeaponTaker> {

    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.WeaponAttackEnter();
    }
    //float DuringTime = 0;// 未开枪姿势持续时间
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnSLStateExit(animator, stateInfo, layerIndex);
        m_MonoBehaviour.WeaponAttckOver();
    }
}


