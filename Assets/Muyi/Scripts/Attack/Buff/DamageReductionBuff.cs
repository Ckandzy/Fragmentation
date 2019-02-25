using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 增益buff

/// <summary>
/// 减伤 -- 降低25%的伤害
/// </summary>
public class DamageReductionBuff : GainBuff<Status>
{
    public DamageReductionBuff(float _buffNum, float _percnetage = 0) : base(_buffNum, _percnetage) { }
    public DamageReductionBuff(int lv):base(lv) { } // 使用lv来控制BuffNum 和 buffpercentage
    public DamageReductionBuff() { buffID = 1; }
    public override void BuffOnEnter(GameObject t)
    {
        TClass = t.GetComponent<Status>();
        buffPercentage = -0.25f;
        this.liveTime = 10;
        TClass.HurtInfluences += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.HurtInfluences -= buffPercentage;
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
        TClass.HurtInfluences -= buffPercentage;
        base.FlushBuff(_num, _percentage);
        TClass.HurtInfluences += buffPercentage;
        
    }

    public override BuffType getBuffType()
    {
        return BuffType.DamageReduction;
    }
}

//
public class AttackMakeSlowDown : AttackTakeBuff<Status>
{
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<Status>();
        liveTime = 10;
        TakeBuff = BuffFactory.GetBuff((int)BuffType.SlowDown);
        TClass.AddAttackCarryingBuff(TakeBuff);
    }

    public override void BuffOver()
    {
        TClass.RemoveAttackCarryingBuff(TakeBuff);
    }

    public override string Des()
    {
        throw new System.NotImplementedException();
    }

    public override void FlushTime(float _time)
    {
        throw new System.NotImplementedException();
    }

    public override BuffEffectType getBuffEffectType()
    {
        throw new System.NotImplementedException();
    }

    public override BuffType getBuffType()
    {
        throw new System.NotImplementedException();
    }
}
