using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
namespace Inv
{
	public class InvAnima : AnimaView {
        public override void OnEnter(UIType _type)
        {
            base.OnEnter(_type);
            animator.SetBool("Enter", true);
            animator.SetBool("Exit", false);
        }

        public override void OnExit(UIType _type)
        {
            animator.SetBool("Enter", false);
            animator.SetBool("Exit", true);
        }

        public override void OnPause(UIType _type)
        {
            base.OnPause(_type);
        }

        public override void OnWake(UIType _type)
        {
            base.OnWake(_type);
        }

        public void Changto(string _type)
        {
            SingleTon<LeftPanelMgr>.Instance.ChangeToUI(getType(_type));
        }

        private InvType getType(string _index)
        {
            switch (_index)
            {
                case "1": return SingleTon<LeftPanelUIMgr>.Instance.IntroductionPanel;
                case "2": return SingleTon<LeftPanelUIMgr>.Instance.SkillPanelPanel;
                case "3": return SingleTon<LeftPanelUIMgr>.Instance.TalentPanel;
            }

            return null;
        }
    }
}


