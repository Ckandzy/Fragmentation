using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
namespace Inv
{
	public class LeftPanelUIMgr : UIManager {
        private LeftPanelUIMgr() { }//Init(); }

        protected override void Init()
        {
            //canves = GameObject.Find("LeftPanel").transform;

            // 这里为了方便采用的不规范写法， 最好使用UItype的创建形式
            //SingleTon<LeftPanelUIMgr>.Instance.GetByUIType();
            //Debug.Log(SingleTon<LeftPanelMgr>.Instance.NotifiedUIDic);
            Debug.Log(LeftPanleAnima.inro);
            //SingleTon<LeftPanelMgr>.Instance.NotifiedUIDic = new Dictionary<UIType, GameObject>();
            SingleTon<LeftPanelMgr>.Instance.NotifiedUIDic.Add(IntroductionPanel, LeftPanleAnima.inro);
            SingleTon<LeftPanelMgr>.Instance.NotifiedUIDic.Add(SkillPanelPanel, LeftPanleAnima.skill); 
            SingleTon<LeftPanelMgr>.Instance.NotifiedUIDic.Add(TalentPanel, LeftPanleAnima.Talen);
            SingleTon<LeftPanelMgr>.Instance.NotifyExcept(IntroductionPanel);
        }

        public readonly InvType IntroductionPanel = new InvType("IntroductionPanel");
        public readonly InvType SkillPanelPanel = new InvType("SkillPanelPanel");
        public readonly InvType TalentPanel = new InvType("TalentPanel");

        
    }
}


