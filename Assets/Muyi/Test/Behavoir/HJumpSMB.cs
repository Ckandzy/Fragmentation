using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class HJumpSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.UpdateJump();
        m_MonoBehaviour.AirborneHorizontalMovement();
        m_MonoBehaviour.AirborneVerticalMovement();
        m_MonoBehaviour.CheckForGrounded();
        //m_MonoBehaviour.move
    }
}


