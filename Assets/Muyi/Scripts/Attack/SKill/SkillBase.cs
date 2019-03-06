using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class SkillBase
{
    public System.Action<bool> OnSkillCDOver;

    public float LeftTime { get { return CD - m_WaitTime; } }
    public List<IBuff> buffs = new List<IBuff>();
    public float CD = 30;
    public float DurationTime = 10; // 持续时间
    public Sprite SkillIcon;
    public SkillStatusEnum MSkillStatus;
    protected float m_WaitTime = 0;
    public abstract void SkillEnter();

    public abstract void SkillUpdate();

    public abstract void OnSkillOver();

    public abstract void UseSkill(Transform _trans);
    public abstract SkillNameEnum SkillName();
}

public enum SkillNameEnum
{
    UnlawlessSkill = 0,
    IronFistPowerSKill = 1,
    WindHelpFire = 2
}

public enum SkillStatusEnum
{
    /// <summary>
    /// 等待CD状态
    /// </summary>
    InCD,
    /// <summary>
    /// 持续状态
    /// </summary>
    Continued,
    /// <summary>
    /// 准备状态
    /// </summary>
    Ready
}
