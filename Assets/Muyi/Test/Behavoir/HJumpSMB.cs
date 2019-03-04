using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class HJumpSMB : SceneLinkedSMB<PlayerCharacter>
{
    int m_JumpCount = 0;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_JumpCount += 1;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.UpdateJump();
        m_MonoBehaviour.AirborneHorizontalMovement();
        m_MonoBehaviour.AirborneVerticalMovement();
        m_MonoBehaviour.CheckForGrounded();

        if (m_MonoBehaviour.CheckForJumpInput() && m_JumpCount < m_MonoBehaviour.CanJumpCount)
        {
            m_MonoBehaviour.SetVerticalMovement(m_MonoBehaviour.jumpSpeed);
            m_JumpCount += 1;
        }
            
        //m_MonoBehaviour.move
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_JumpCount = 0;
    }
}


