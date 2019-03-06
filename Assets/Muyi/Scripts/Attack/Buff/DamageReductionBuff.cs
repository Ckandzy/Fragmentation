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
        Over = false;
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
        buffPercentage = -0.25f * LV;
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
        Over = false;
        TClass = obj.GetComponent<Status>();
        liveTime = 10;
        TakeBuff = BuffFactory.GetBuff(2, 1);
        TClass.AddAttackCarryingBuff(TakeBuff);
        Over = false;
    }

    public override void BuffOver()
    {
        TClass.RemoveAttackCarryingBuff(TakeBuff);
        Over = true;
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
        Over = false;
        TClass = obj.GetComponent<Status>();
        BlowoutEffect = Resources.Load<GameObject>("VFX/Blowout/Blowout");
        foreach(TakeDamager damager in TClass.TakeDamagers)
        {
            damager.OnDamageableHit.AddListener(Blowout);
        }
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
        Over = true;
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
        Over = false;
    }

    public override void BuffOver()
    {
        TClass.SpikeRate -= buffPercentage;
        Over = true;
    }

    public override void CalculationBuffNum()
    {
        buffPercentage = 0.1f * LV;
        liveTime = 6 ;
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
        Over = false;
    }

    public override void BuffOver()
    {
        TClass.RemoveAttackCarryingBuff(TakeBuff);
        Over = true;
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
    public GetTwoSegmentJump() { buffID = 6; }
    public GetTwoSegmentJump(int lv, bool _permanent = false) : base(lv) { buffID = 6; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        TClass = obj.GetComponent<PlayerCharacter>();
        CalculationBuffNum();
        TClass.CanJumpCount += (int)buffNum;
        Over = false;
    }

    public override void BuffOver()
    {
        TClass.CanJumpCount -= (int)buffNum;
        Over = true;
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

// 弹射
// 使用弹射buff时-必须要有VFX_lightning场景中
public class AtttakMakeCatapult : GainBuff<Status>
{
    public AtttakMakeCatapult() { buffID = 6; }
    public AtttakMakeCatapult(int lv, bool _permanent = false) : base(lv) { buffID = 6; permanent = _permanent; }
    private int CatapultNum = 0;
   
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        List<TakeDamager> takeDamagers = TClass.TakeDamagers;
        for (int i = 0; i < takeDamagers.Count; i++)
        {
            takeDamagers[i].OnDamageableHit.AddListener(Calculat);
        }
    }

    public override void BuffOver()
    {
        Over = true;
        List<TakeDamager> takeDamagers = TClass.TakeDamagers;
        for (int i = 0; i < takeDamagers.Count; i++)
        {
            takeDamagers[i].OnDamageableHit.RemoveListener(Calculat);
        }
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = 10;
    }

    /// <summary>
    /// 目前的弹射未携带之前的攻击特效 -- 同时可以优化，缓存GameObject.FindGameObjectsWithTag("Enemy");
    /// 不用每次都计算
    /// </summary>
    /// <param name="_damager"></param>
    /// <param name="_damageable"></param>
    public void Calculat(TakeDamager _damager, TakeDamageable _damageable)
    {
        GameObject calculatObj = FindOtherEnemyInRange(_damageable.transform);
        if (calculatObj == null) return;
        VFXControllerM.Instance.MakeLighting(_damageable.transform, calculatObj.transform, 0.2f);
        calculatObj.GetComponent<TakeDamageable>().TakeDamage(_damager);
    }

    private GameObject FindOtherEnemyInRange(Transform _start)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in gameObjects)
        {
            if (obj == _start.gameObject) continue;
            if((obj.transform.position - _start.position).sqrMagnitude <= 49)
            {
                return obj;
            }
        }
        return null;
    }

    public override string Des()
    {
        return "弹射伤害";
    }

    public override BuffType getBuffType()
    {
        return BuffType.AtttakMakeCatapult;
    }
}

/// <summary>
/// 血量下降
/// </summary>
public class HPDown : NegativeBuff<Status>
{

    public HPDown() { buffID = 6; }
    public HPDown(int lv, bool _permanent = false) : base(lv) { buffID = 6; permanent = _permanent; }
    float MaxHp;
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        MaxHp = TClass.MaxHP;
        TClass.MaxHP *= (1 + buffPercentage);
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.MaxHP = MaxHp;
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = 6 * LV;
        buffPercentage = -0.25f * LV;
    }

    public override string Des()
    {
        return "降低最大生命值";
    }

    public override BuffType getBuffType()
    {
        return BuffType.HPDown;
    }
}

/// <summary>
/// 提高移动速度
/// </summary>
public class UpSpeed : GainBuff<PlayerCharacter>
{
    public UpSpeed() { buffID = 6; }
    public UpSpeed(int lv, bool _permanent = false) : base(lv) { buffID = 6; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<PlayerCharacter>();
        CalculationBuffNum();
        TClass.maxSpeed *= (1 + buffPercentage);
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.maxSpeed /= (1 + buffPercentage); 
    }

    public override void CalculationBuffNum()
    {
        buffPercentage = 0.3f;
        nowTime = 0;
        liveTime = 10 * LV;
    }

    public override string Des()
    {
        return "提高移动速度";
    }

    public override BuffType getBuffType()
    {
        return BuffType.UpSpeed;
    }
}

/// <summary>
/// 吸血
/// </summary>
public class Bloodsucking : GainBuff<Status>
{

    public Bloodsucking() { buffID = 6; }
    public Bloodsucking(int lv, bool _permanent = false) : base(lv) { buffID = 6; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        TClass.BloodsuckingRate += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.BloodsuckingRate -= buffPercentage;
    }

    public override void CalculationBuffNum()
    {
        buffPercentage = 0.2f * LV;
        nowTime = 0;
        liveTime = 8 * LV;
    }

    public override string Des()
    {
        return "提高吸血";
    }

    public override BuffType getBuffType()
    {
        return BuffType.Bloodsucking;
    }
}

/// <summary>
/// 受到的伤害提高
/// </summary>
public class DamageableUpBuff : NegativeBuff<Status>
{
    public DamageableUpBuff() { buffID = 1; }
    public DamageableUpBuff(int lv, bool _permanent = false) : base(lv) { buffID = 1; permanent = _permanent; }
    public override void BuffOnEnter(GameObject t)
    {
        Over = false;
        TClass = t.GetComponent<Status>();
        this.CalculationBuffNum();
        TClass.HurtInfluences += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.HurtInfluences -= buffPercentage;
    }

    public override string Des()
    {
        return "提高" + (buffPercentage == 0 ? buffNum : buffPercentage) + "伤害";
    }

    public override BuffType getBuffType()
    {
        return BuffType.DamageableUp;
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        buffPercentage = 0.25f * LV;
        liveTime = 5 * LV;
    }
}

/// <summary>
/// 提高攻击力
/// </summary>
public class AttackNumUp : GainBuff<Status>
{
    public AttackNumUp() { buffID = 1; }
    public AttackNumUp(int lv, bool _permanent = false) : base(lv) { buffID = 1; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        TClass.TakeDamageInfluences += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.TakeDamageInfluences -= buffPercentage;
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = 10 * LV;
        buffPercentage = 0.2f * LV;
    }

    public override string Des()
    {
        throw new System.NotImplementedException();
    }

    public override BuffType getBuffType()
    {
        return BuffType.AttackNumUp;
    }
}

/// <summary>
/// 提高闪避率
/// </summary>
public class UpDodgeRate : GainBuff<Status>
{
    public UpDodgeRate() { buffID = 1; }
    public UpDodgeRate(int lv, bool _permanent = false) : base(lv) { buffID = 1; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
        TClass.DodgeRate += buffPercentage;
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.DodgeRate -= buffPercentage;
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = 4 * LV;
        buffPercentage = 0.1f * LV;
    }

    public override string Des()
    {
        return "提高闪避率";
    }

    public override BuffType getBuffType()
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// 灼烧
/// </summary>
public class Burning : NegativeBuff<Status>
{
    public Burning() { buffID = 1; }
    public Burning(int lv, bool _permanent = false) : base(lv) { buffID = 1; permanent = _permanent; }
    public override void BuffOnEnter(GameObject obj)
    {
        Over = false;
        TClass = obj.GetComponent<Status>();
        CalculationBuffNum();
    }

    public override void BuffUpdate()
    {
        base.BuffUpdate();
        TClass.HP *= (1 - buffPercentage) * Time.deltaTime;
    }

    public override void BuffOver()
    {
        Over = true;
        //TClass.HP *= (1 - buffPercentage);
    }

    public override void CalculationBuffNum()
    {
        nowTime = 0;
        liveTime = 5;
        switch (TClass.StatusType)
        {
            // 一共15%， 共5s
            case CharacterType.Boss: buffPercentage = TClass.AttackDamageNum * 0.15f / 5; break;
            case CharacterType.EliteEnemy:
            case CharacterType.Player:
            case CharacterType.LowLevelEnemy: buffPercentage = TClass.AttackDamageNum * 0.1f / 5; break;
            
        }
    }

    public override string Des()
    {
        return "每秒灼烧血量";
    }

    public override BuffType getBuffType()
    {
        return BuffType.Burning;
    }
}

/// <summary>
/// 攻击具有灼烧效果
/// </summary>
public class AttackMakeBurning : AttackTakeBuff<Status>
{
    public AttackMakeBurning() { buffID = 1; }
    public AttackMakeBurning(int lv, bool _permanent = false) : base(lv) { buffID = 1; permanent = _permanent; }
    IBuff takeBuff;
    public override void BuffOnEnter(GameObject obj)
    {
        takeBuff = new Burning(LV, true);
        Over = false;
        CalculationBuffNum();
        TClass.AddAttackCarryingBuff(takeBuff);
    }

    public override void BuffOver()
    {
        Over = true;
        TClass.RemoveAttackCarryingBuff(takeBuff);
    }

    public override void CalculationBuffNum()
    {
        liveTime = 10 + LV * 2;
        nowTime = 0;
    }

    public override string Des()
    {
        return "攻击具有灼烧效果";
    }

    public override BuffType getBuffType()
    {
        return BuffType.AttackMakeBurning;
    }
}