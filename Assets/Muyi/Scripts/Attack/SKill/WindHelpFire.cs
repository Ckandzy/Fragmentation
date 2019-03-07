using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风助火威
/// </summary>
public class WindHelpFire : SkillBase
{
    public string SkillObjPath;
    public WindHelpFire() { }

    float durationTimer = 0;
    public override void SkillEnter()
    {
        MSkillStatus = SkillStatusEnum.Ready;
        CD = 40;
        DurationTime = 5;
        SkillIcon = Resources.Load<Sprite>("SkillIcon/WindHelpFire");
        buffs.Add(BuffFactory.GetBuff((int)BuffType.UpSpikeRate, 9, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.AttackNumUp, 3, true));
    }

    public override void SkillUpdate()
    {
        if (MSkillStatus == SkillStatusEnum.Ready)
        {
            m_WaitTime = 0;
            durationTimer = 0;
        }
        else if (MSkillStatus == SkillStatusEnum.InCD)
        {
            m_WaitTime += Time.deltaTime;
            if (m_WaitTime >= CD)
            {
                MSkillStatus = SkillStatusEnum.Ready;
                if (OnSkillCDOver != null) OnSkillCDOver(true);
            }
        }
        else if (MSkillStatus == SkillStatusEnum.Continued)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer >= DurationTime)
            {
                MSkillStatus = SkillStatusEnum.InCD;
                OnSkillOver();
            }
        }
    }

    public override SkillNameEnum SkillName()
    {
        return SkillNameEnum.WindHelpFire;
    }

    Status status;
    public override void OnSkillOver()
    {
        status.RemoveStatuBuff(buffs);
    }

    public override void UseSkill(Transform _trans)
    {
        if (MSkillStatus == SkillStatusEnum.Ready)
        {
            MSkillStatus = SkillStatusEnum.Continued;
            m_WaitTime = 0;
            status = _trans.parent.GetComponent<Status>();
            status.AddStatusBuff(buffs);
        }
    }

    public override string Des()
    {
        return "风助火威： 玩家闪避成功的几率提升至90%，攻击力增加至150%，持续5秒";
    }
}
