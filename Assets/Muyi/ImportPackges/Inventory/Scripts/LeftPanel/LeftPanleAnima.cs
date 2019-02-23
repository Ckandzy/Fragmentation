using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
namespace Inv
{
    [RequireComponent(typeof(Animator))]
	public class LeftPanleAnima : AnimaView {

        public static GameObject inro;
        public static GameObject skill;
        public static GameObject Talen;

        private void Awake()
        {
            inro = GameObject.Find("IntroductionPanel");
            skill = GameObject.Find("SkillPanelPanel");
            Talen = GameObject.Find("TalentPanel");

            SingleTon<LeftPanelMgr>.Instance.Init();
        }

        public override void OnEnter(UIType _type)
        {
            //base.OnEnter(_type);
            
        }

        public override void OnExit(UIType _type)
        {
            base.OnExit(_type);
        }

        public override void OnPause(UIType _type)
        {
            base.OnPause(_type);
        }

        public override void OnWake(UIType _type)
        {
            base.OnWake(_type);
        }


    }
}


