using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
namespace Inv
{
	public class GameRoot : MonoBehaviour {
        private void Awake()
        {
            //SingleTon<InvUIMgr>.Instance.get
            SingleTon<UIMgr>.Instance.Push(InvUI);
            SingleTon<UIMgr>.Instance.Push(LeftPanleUI);
            //SingleTon<UIMgr>.Instance.Push(EquipInvUI);
        }


        public static readonly InvType InvUI = new InvType("InventoryFrame");
        public static readonly InvType EquipInvUI = new InvType("Equip_Container");
        public static readonly InvType LeftPanleUI = new InvType("LeftPanel");
    }
}


