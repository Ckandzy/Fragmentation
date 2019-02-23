using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;

namespace Inv
{
	public class InvUIMgr : UIManager{
        private InvUIMgr() { Init(); }

        protected override void Init()
        {
            canves = GameObject.Find("BasePanel").transform;
        }
    }
}


