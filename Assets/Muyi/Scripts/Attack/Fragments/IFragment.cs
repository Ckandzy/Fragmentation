using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IFragment{
    public int ID;
    public int LV = 1;
    //public List<int> BuffIds = new List<int>();
    public List<IBuff> buffs = new List<IBuff>();
    public abstract FragmentType GetFragType();
    public abstract string Des();
    public abstract FragmentName GetFragName();
}

/// <summary>
/// 守护者碎片
/// </summary>
public abstract class IGuardFrag : IFragment
{
    public override FragmentType GetFragType()
    {
        return FragmentType.Guard;
    }
}

/// <summary>
/// 恶魔碎片
/// </summary>
public abstract class IDemonFrag : IFragment
{
    public override FragmentType GetFragType()
    {
        return FragmentType.Demon;
    }
}

/// <summary>
/// 元碎片
/// </summary>
public abstract class INaturalFrag : IFragment
{
    public override FragmentType GetFragType()
    {
        return FragmentType.Natural;
    }
}

public enum FragmentType
{
   /// <summary>
   /// 守护者碎片
   /// </summary>
    Guard,
    /// <summary>
    /// 恶魔碎片
    /// </summary>
    Demon,
    /// <summary>
    /// 元素碎片
    /// </summary>
    Natural,
    Null
}

public enum FragmentName
{
    /// <summary>
    /// 怒焰
    /// </summary>
    AngryFlame,
    /// <summary>
    /// 审判
    /// </summary>
    TrialFrag,
    /// <summary>
    /// 震慑
    /// </summary>
    ShockFrag,
    /// <summary>
    /// 准绳
    /// </summary>
    CriterionFrag,
    /// <summary>
    /// 无序
    /// </summary>
    Disorder,
    /// <summary>
    /// 狂躁
    /// </summary>
    Arrogant,
    /// <summary>
    /// 暴乱
    /// </summary>
    Riot,
    /// <summary>
    /// 风
    /// </summary>
    WindFrag,
    /// <summary>
    /// 火
    /// </summary>
    FireFrag,
    Null

}


