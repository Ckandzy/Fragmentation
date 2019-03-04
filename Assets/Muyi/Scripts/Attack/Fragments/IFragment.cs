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
    Natural
}


