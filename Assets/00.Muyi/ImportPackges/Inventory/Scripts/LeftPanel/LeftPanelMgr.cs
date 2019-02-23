using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using System;

namespace Inv
{
    // 处理多个并行UI， 且随时可以跳转，但只能存在一个，如同仓库的分类，武器，材料等并行,但
    // 使用事件通知机制
    // 当跳转到一个UI时， 由Mgr通知其余所有的UI这个事件， 做出处理
    // page Mgr
	public class LeftPanelMgr : NotifyPageManager {
        private LeftPanelMgr() { }
        
        //public void Add(UIType _type)
        //{
        //    if (NotifiedUIDic.ContainsKey(_type)) return;
        //    //GameObject obj = SingleTon<LeftPanelUIMgr>.Instance.GetPageGameObj(_type);
        //}

        public void Init()
        {
            LeftPanelUIMgr LeftMgr = SingleTon<LeftPanelUIMgr>.Instance;
           // Add(LeftMgr.IntroductionPanel);
        }

        public override void ChangeToUI(UIType _type)
        {
            base.ChangeToUI(_type);
        }
    }
}


