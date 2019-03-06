using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 铁腕强权
/// 铁腕强权（冷却时间40秒）
/// 准绳+审判碎片激活
/// 效果：秒杀全场杂兵，对精英敌人造成最高不超过其总血量20%的伤害，对boss造成最高不超过其总
/// 血量3%的伤害
/// </summary>
public class IronFistPowerSKill : SkillBase
{
    public string SkillObjPath;
    public IronFistPowerSKill() { }

    float durationTimer = 0;
    public override void SkillEnter()
    {
        MSkillStatus = SkillStatusEnum.Ready;
        CD = 40;
        DurationTime = 3;
        SkillIcon = Resources.Load<Sprite>("SkillIcon/IronFistPowerSKill");
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

    public override void OnSkillOver()
    {
        SkillKill();
    }

    public override void UseSkill(Transform _trans)
    {
        if (MSkillStatus == SkillStatusEnum.Ready)
        {
            MSkillStatus = SkillStatusEnum.Continued;
            m_WaitTime = 0;
            //SkillKill();
        }
    }

    private void SkillKill()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        if (objects == null || objects.Length == 0) return;
        foreach(GameObject obj in objects)
        {
            Status status = obj.GetComponent<Status>();
            switch (status.StatusType)
            {
                case CharacterType.LowLevelEnemy: status.HP -= float.MaxValue;break;
                case CharacterType.EliteEnemy: status.HP -= status.MaxHP * (1 - 0.2f); break;
                case CharacterType.Boss: status.HP -= status.MaxHP * (1-0.03f); break;
            }
        }
    }

    public override SkillNameEnum SkillName()
    {
        return SkillNameEnum.IronFistPowerSKill;
    }
}
