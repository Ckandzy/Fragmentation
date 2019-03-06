using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class HJumpAttackSMB : SceneLinkedSMB<WeaponTaker>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.CheckForHoldingGun();
        if(PlayerInput.Instance.Attack.Down)
            m_MonoBehaviour.Attack ();
        //m_MonoBehaviour.CheckAndFireGun ();
        //m_MonoBehaviour.CheckForCrouching ();
        if (PlayerInput.Instance.UseSkill.Down)
        {
            m_MonoBehaviour.UseSkill();
        }
    }
}


