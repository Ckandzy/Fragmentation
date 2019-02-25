using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class HLocomotionSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.TeleportToColliderBottom();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.GroundedHorizontalMovement(true);
        m_MonoBehaviour.GroundedVerticalMovement();
        m_MonoBehaviour.CheckForGrounded();

        if (m_MonoBehaviour.CheckForJumpInput())
            m_MonoBehaviour.SetVerticalMovement(m_MonoBehaviour.jumpSpeed);
    }
}


