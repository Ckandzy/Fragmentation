using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
    // 使用此Frame时， ui的位置可以不用调， 通过动画来控制位置
	public abstract class BaseView : MonoBehaviour{
        public virtual void OnEnter(UIType _type) { } // 进入
        public virtual void OnExit(UIType _type) { } // 退出
        public virtual void OnPause(UIType _type) { } // 暂停
        public virtual void OnWake(UIType _type) { } // 暂停后唤醒

    }
}


