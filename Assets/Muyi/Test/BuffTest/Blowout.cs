using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowout : BlowoutSMB<GameObject>
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Destroy(TClass.gameObject);
    }
}

public class BlowoutSMB<T> : StateMachineBehaviour
{
    public T TClass;

    public static void Init(Animator animator, T _t)
    {
        foreach(BlowoutSMB<T> behaviour in animator.GetBehaviours<BlowoutSMB<T>>())
        {
            behaviour.TClass = _t;
        }
    }
}
