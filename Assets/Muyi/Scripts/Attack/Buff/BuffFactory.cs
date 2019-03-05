using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="lv"></param>
    /// <param name="forever">是否永久持有，不进行计时</param>
    /// <returns></returns>
    public static IBuff GetBuff(int index, int lv = 1, bool forever = false)
    {
        switch (index)
        {
            case 1: return new DamageReductionBuff(lv, forever);
            case 2: return new SlowDown(lv, forever);
            case 3: return new AttackMakeBlowout(lv, forever);
            case 4: return new AttackPowerReduce(lv, forever);
            case 5: return new UpSpikeRate(lv, forever);
            case 6: return new Frozen(lv, forever);
            case 7: return new AttakeMakeFrozen(lv, forever);
            case 8: return new GetTwoSegmentJump(lv, forever);
            case 9: return new AtttakMakeCatapult(lv, forever);
            case 10: return new HPDown(lv, forever);
            case 11: return new UpSpeed(lv, forever);
            case 12: return new Bloodsucking(lv, forever);
            case 13: return new DamageableUpBuff(lv, forever);
            case 14: return new AttackNumUp(lv, forever);
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
    GetTwoSegmentJump = 8,
    /// <summary>
    /// 攻击弹射
    /// </summary>
    AtttakMakeCatapult = 9,
    /// <summary>
    /// 血量下降
    /// </summary>
    HPDown = 10,
    /// <summary>
    /// 移速增加
    /// </summary>
    UpSpeed = 11,
    /// <summary>
    /// 吸血
    /// </summary>
    Bloodsucking = 12,
    /// <summary>
    /// 受到的伤害提高
    /// </summary>
    DamageableUp = 13,
    /// <summary>
    /// 提高攻击力
    /// </summary>
    AttackNumUp = 14
}

public enum BuffEffectType
{
    Negative, // 负面
    Gain, // 增益
}


