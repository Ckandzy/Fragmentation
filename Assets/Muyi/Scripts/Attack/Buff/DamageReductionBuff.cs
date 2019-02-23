using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 减伤 -- 降低25%的伤害
/// </summary>
public class DamageReductionBuff : IBuff<PlayerStatus>
{
    public DamageReductionBuff(float _buffNum, float _percnetage = 0) : base(_buffNum, _percnetage) { }
    public DamageReductionBuff(int lv):base(lv) { } // 使用lv来控制BuffNum 和 buffpercentage
    public DamageReductionBuff() { buffID = 1; }
    public override void BuffOnEnter(GameObject t)
    {
        TClass = t.GetComponent<PlayerStatus>();
        buffPercentage = -0.25f;
        TClass.DamageInfluences += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.DamageInfluences -= buffPercentage;
    }

    public override void BuffUpdate()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= liveTime) BuffOver();
    }

    public override string Des()
    {
        return "降低" + (buffPercentage == 0 ? buffNum : buffPercentage) + "伤害";
    }

    public override void FlushTime(float _time)
    {
        throw new System.NotImplementedException();
    }

    public override void FlushBuff(float _num, float _percentage = 0)
    {
        TClass.DamageInfluences -= buffPercentage;
        base.FlushBuff(_num, _percentage);
        TClass.DamageInfluences += buffPercentage;
        
    }

    public override BuffType getBuffType()
    {
        return BuffType.DamageReduction;
    }
}


