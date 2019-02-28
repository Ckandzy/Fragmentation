using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
// 增益buff

/// <summary>
/// 减伤 -- 降低25%的伤害
/// </summary>
public class DamageReductionBuff : GainBuff<Status>
{
    public DamageReductionBuff() { buffID = 1; }
    public DamageReductionBuff(int lv, bool _permanent = false):base(lv) { buffID = 1; permanent = _permanent; }
    public override void BuffOnEnter(GameObject t)
    {
        TClass = t.GetComponent<Status>();
        this.CalculationBuffNum();
        TClass.HurtInfluences += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        Debug.Log(TClass);
        TClass.HurtInfluences -= buffPercentage;
    }

    public override string Des()
    {
        return "降低" + (buffPercentage == 0 ? buffNum : buffPercentage) + "伤害";
    }

    public override void FlushTime(float _time)
    {
        throw new System.NotImplementedException();
    }

    public override BuffType getBuffType()
    {
        return BuffType.DamageReduction;
    }

    public override BuffEffectType getBuffEffectType()
    {
        if (buffPercentage > 0 || buffNum > 0) return BuffEffectType.Gain;
        else return BuffEffectType.Negative;
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        buffPercentage = -0.1f * LV;
        liveTime = 5 * LV;
    }
}

// 攻击造成减速
public class AttackMakeSlowDown : AttackTakeBuff<Status>
{
    public AttackMakeSlowDown() { buffID = 1; }
    public AttackMakeSlowDown(int lv, bool _permanent = false) : base(lv) { buffID = 1; permanent = _permanent; }

    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<Status>();
        liveTime = 10;
        TakeBuff = BuffFactory.GetBuff(2, 1);
        TClass.AddAttackCarryingBuff(TakeBuff);
    }

    public override void BuffOver()
    {
        //TClass.RemoveAttackCarryingBuff(TakeBuff);
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = 10 * LV;
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

// 攻击具有小范围爆炸效果
public class AttackMakeBlowout : AttackTakeBuff<Status>
{

    public AttackMakeBlowout() { buffID = 3; }
    public AttackMakeBlowout(int lv, bool _permanent = false) : base(lv) { buffID = 3; permanent = _permanent; }
    public GameObject BlowoutEffect;
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<Status>();
        BlowoutEffect = Resources.Load<GameObject>("VFX/Blowout/Blowout");
        foreach(TakeDamager damager in TClass.TakeDamagers)
        {
            damager.OnDamageableHit.AddListener(Blowout);
        }
        Debug.Log(TClass.TakeDamagers.Count);
    }

    public void Blowout(TakeDamager damager, TakeDamageable damageable)
    {
        GameObject vfx = MonoBehaviour.Instantiate(BlowoutEffect);
        vfx.transform.position = damageable.AttackPoint;
    }

    public override void BuffOver()
    {
        foreach (TakeDamager damager in TClass.TakeDamagers)
        {
            damager.OnDamageableHit.RemoveListener(Blowout);
        }
    }

    public override void CalculationBuffNum()
    {
        
    }

    public override string Des()
    {
        throw new System.NotImplementedException();
    }

    public override BuffEffectType getBuffEffectType()
    {
        return BuffEffectType.Gain;
    }

    public override BuffType getBuffType()
    {
        return BuffType.AttackMakeBlowout;
    }
}

// 提高秒杀率
public class UpSpikeRate : GainBuff<Status>
{
    public UpSpikeRate() : base() { buffID = 5; }
    public UpSpikeRate(int lv, bool _permanent = false) : base(lv) { buffID = 5; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        TClass.SpikeRate += buffPercentage;
    }

    public override void BuffOver()
    {
        TClass.SpikeRate -= buffPercentage;
    }

    public override void CalculationBuffNum()
    {
        buffPercentage = 0.1f;
        liveTime = 10 * LV;
        nowTime = 0;
    }

    public override string Des()
    {
        return "提高秒杀率";
    }

    public override BuffType getBuffType()
    {
        return BuffType.UpSpikeRate;
    }
}

// 攻击具有冰冻效果
public class AttakeMakeFrozen : AttackTakeBuff<Status>
{
    public AttakeMakeFrozen() { buffID = 6; }
    public AttakeMakeFrozen(int lv, bool _permanent = false) : base(lv) { buffID = 6; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        TClass.AddAttackCarryingBuff(TakeBuff);
    }

    public override void BuffOver()
    {
        TClass.RemoveAttackCarryingBuff(TakeBuff);
    }

    public override void CalculationBuffNum()
    {
        liveTime = 10 * LV;
        TakeBuff = BuffFactory.GetBuff(6);
    }

    public override string Des()
    {
        return "攻击携带冰冻效果";
    }

    public override BuffType getBuffType()
    {
        return BuffType.AttakeMakeFrozen;
    }
    public override BuffEffectType getBuffEffectType()
    {
        return BuffEffectType.Gain;
    }
}

// 获得2段跳技能
public class GetTwoSegmentJump : GainBuff<PlayerCharacter>
{
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<PlayerCharacter>();
        CalculationBuffNum();
        TClass.CanJumpCount += (int)buffNum;
    }

    public override void BuffOver()
    {
        TClass.CanJumpCount -= (int)buffNum;
    }

    public override void CalculationBuffNum()
    {
        liveTime = 10;
        nowTime = 0;
        buffNum = 1;
    }

    public override string Des()
    {
        return "获得2段跳技能";
    }

    public override BuffType getBuffType()
    {
        return BuffType.GetTwoSegmentJump;
    }

    public override BuffEffectType getBuffEffectType()
    {
        return BuffEffectType.Gain;
    }
}
