using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
using BTAI;

public class BotBT : MonoBehaviour
#if UNITY_EDITOR
    , BTAI.IBTDebugable
#endif
{
    Animator m_Animator;
    Damageable m_Damageable;
    Root m_Ai = BT.Root();
    EnemyBehaviour m_EnemyBehaviour;

    private void OnEnable()
    {
        m_EnemyBehaviour = GetComponent<EnemyBehaviour>();
        m_Animator = GetComponent<Animator>();

        m_Ai.OpenBranch(
            BT.If(() => { return m_EnemyBehaviour.Target != null; }).OpenBranch(
                BT.Call(m_EnemyBehaviour.CheckTargetStillVisible),
                BT.Call(m_EnemyBehaviour.OrientToTarget),
                BT.If(() => { return m_EnemyBehaviour.CheckLaunchMode() == true; }).OpenBranch(
                    BT.Call(() => { m_EnemyBehaviour.StartLaunch(true); })
                    //BT.WaitForAnimatorState(m_Animator, "BotAttack")
                ),
                BT.If(() => { return m_EnemyBehaviour.CheckLaunchMode() == false; }).OpenBranch(
                    BT.Call(() => { m_EnemyBehaviour.StartLaunch(false); })
                    //BT.WaitForAnimatorState(m_Animator, "BotAttack2")
                ),
                BT.Call(m_EnemyBehaviour.RememberTargetPos)
            ),

            BT.If(() => { return m_EnemyBehaviour.Target == null; }).OpenBranch(
                BT.Call(m_EnemyBehaviour.ScanForPlayer)
            )
        );
    }

    private void Update()
    {
        m_Ai.Tick();
    }
#if UNITY_EDITOR
    public BTAI.Root GetAIRoot()
    {
        return m_Ai;
    }
#endif
}
