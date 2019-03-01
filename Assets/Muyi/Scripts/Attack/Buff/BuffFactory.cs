using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffFactory
{
    public static IBuff GetBuff(int index, int lv = 1, bool forever = false)
    {
        switch (index)
        {
            case 1: return new DamageReductionBuff(lv, forever);
            case 2: return new SlowDown(lv, forever);
            case 3: return new AttackMakeBlowout(lv, forever);
            case 4: return new AttackPowerReduce(lv, forever);
            case 5: return new UpSpikeRate(lv, forever);
        }
        throw new System.Exception("no this buff, but try get it");
    }
}

public enum BuffType
{
    /// <summary>
    /// 减伤
    /// </summary>
    DamageReduction = 1, 
    /// <summary>
    /// 减速
    /// </summary>
    SlowDown = 2, // 减速
    /// <summary>
    /// 攻击具有小范围爆炸伤害
    /// </summary>
    AttackMakeBlowout = 3, 
    /// <summary>
    /// 减低攻击力
    /// </summary>
    AttackPowerReduce = 4,
    /// <summary>
    /// 提高秒杀率
    /// </summary>
    UpSpikeRate = 5,
    /// <summary>
    /// 冰冻
    /// </summary>
    Frozen = 6,
    /// <summary>
    /// 攻击携带冰冻buff
    /// </summary>
    AttakeMakeFrozen = 7,
    /// <summary>
    /// 获得2段跳技能
    /// </summary>
    GetTwoSegmentJump
}

public enum BuffEffectType
{
    Negative, // 负面
    Gain, // 增益
}


