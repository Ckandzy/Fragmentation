using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase
{
    public bool ReadyUse = true;
    public float LeftTime { get { return CD - m_WaitTime; } }
    public List<IBuff> buffs = new List<IBuff>();
    public float CD = 30;

    protected float m_WaitTime = 0;

    public abstract void SkillUpdate();

    public abstract void OnSkillOver();

    public abstract void UseSkill(Transform _trans);
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
