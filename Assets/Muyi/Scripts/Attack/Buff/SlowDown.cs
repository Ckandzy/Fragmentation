using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

// 负面buff
// 减速 -- 需要改 ，目前只对人物有效
public class SlowDown : NegativeBuff<PlayerCharacter>
{
    public SlowDown() : base() { buffID = 2; }
    public SlowDown(int lv, bool _permanent = false) : base(lv) { buffID = 2; permanent = _permanent; }
    protected float initalSpeed = 0;

    public override void BuffOnEnter(GameObject t)
    {
        TClass = t.GetComponent<PlayerCharacter>();
        CalculationBuffNum();

        if (buffPercentage != 0) { TClass.maxSpeed *= (1 + buffPercentage); }
        else if (buffNum != 0) { TClass.maxSpeed += buffNum; }
    }

    public override void BuffOver()
    {
        TClass.maxSpeed = initalSpeed;
        Over = true;
    }

    public override string Des()
    {
        return "造成减速效果 " + buffPercentage + "%" + " 持续时间 " + liveTime;
    }

    public override BuffType getBuffType()
    {
        return BuffType.SlowDown;
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = LV * 2f;
        buffPercentage = -0.4f * LV;
       
        initalSpeed = TClass.maxSpeed;
    }

    public override BuffEffectType getBuffEffectType()
    {
        if (buffPercentage > 0 || buffNum > 0) return BuffEffectType.Gain;
        else return BuffEffectType.Negative;
    }
}

// 攻击力降低  reduce :  降低
public class AttackPowerReduce : NegativeBuff<Status>
{
    public AttackPowerReduce() : base() { buffID = 4; }
    public AttackPowerReduce(int lv, bool _permanent = false) : base(lv) { buffID = 4; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        TClass.TakeDamageInfluences += buffPercentage;
    }

    public override void BuffOver()
    {
        TClass.TakeDamageInfluences -= buffPercentage;
    }

    public override void CalculationBuffNum()
    {
        buffPercentage = LV * -0.15f;
        liveTime = LV * 6f;
        nowTime = 0;
    }

    public override string Des()
    {
        return "减低攻击力";
    }

    public override BuffType getBuffType()
    {
        return BuffType.AttackPowerReduce;
    }

    public override BuffEffectType getBuffEffectType()
    {
        if (buffPercentage > 0) return BuffEffectType.Gain;
        else return BuffEffectType.Negative;
    }
}

// 冰冻
public class Frozen : SlowDown
{
    public Frozen() : base() { buffID = 2; }
    public Frozen(int lv, bool _permanent = false) : base(lv) { buffID = 2; permanent = _permanent; }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = LV * 3f;
        buffPercentage = -1f * LV;

        initalSpeed = TClass.maxSpeed;
    }
}

