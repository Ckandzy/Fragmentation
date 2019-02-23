using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class HMelleAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.ForceNotHoldingGun();
        //m_MonoBehaviour.EnableMeleeAttack();
        //m_MonoBehaviour.SetHorizontalMovement(m_MonoBehaviour.meleeAttackDashSpeed * m_MonoBehaviour.GetFacing());
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.UpdateJump();
        m_MonoBehaviour.AirborneHorizontalMovement();
        m_MonoBehaviour.AirborneVerticalMovement();
        m_MonoBehaviour.CheckForGrounded();

        //if (!m_MonoBehaviour.CheckForGrounded ())
        //    animator.Play (m_HashAirborneMeleeAttackState, layerIndex, stateInfo.normalizedTime);
        //if (m_MonoBehaviour.CheckForGrounded())
        //    m_MonoBehaviour.GroundedHorizontalMovement (false);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.DisableMeleeAttack();
    }
}


