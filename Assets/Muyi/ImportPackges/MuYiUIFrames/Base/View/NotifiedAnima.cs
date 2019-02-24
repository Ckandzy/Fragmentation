using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inv;

namespace MuyiFrame
{
    // 并行UI
    // 使用事件管理机制控制UI时的Anima
	public abstract class NotifiedAnima : AnimaView {
        public override void OnEnter(UIType _type)
        {
            base.OnEnter(_type);
        }

        // 不使用接口的原因是因为getcomponet<> 函数不能获取到接口函数
        public abstract void Notify(UIType uIType);
    }
}


