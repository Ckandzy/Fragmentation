using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Status : MonoBehaviour {
    [System.Serializable]
    public class BuffEvent : UnityEvent<IBuff> { }

    public float MaxHP = 100;
    public float HP = 100;

    public float HurtInfluences = 1;// influences of hurt such as Damage reduction 20%

    public float AttackDamageNum = 1; // 攻击力
    public float TakeDamageInfluences = 1; // influences of DamageNum 

    public float Precision = 1; // 精准度
    public float DodgeRate = 0; // 闪避率

    public BuffEvent OnStatusBuffAdd;
    public BuffEvent OnAttackCarryingBuffAdd;

    // 当前身上的状态buff -- 如受到减速
    public List<IBuff> StatusBuffs = new List<IBuff>();

    // 当前身上影响攻击的buff -- 如攻击携带减速效果
    public List<IBuff> AttackCarryingBuffs = new List<IBuff>();
    
    public bool isStoic = false;

    TakeDamager takeDamager;

    private void Awake()
    {
        takeDamager = GetComponent<TakeDamager>();
    }

    private void Update()
    {
        for (int i = 0; i < StatusBuffs.Count; i++)
        {
            StatusBuffs[i].BuffUpdate();
            if (StatusBuffs[i].Over)
            {
                StateUIMgr.Instance.RemoveBuff(StatusBuffs[i]);
                StatusBuffs.Remove(StatusBuffs[i]);
            }
        }

        foreach(IBuff buff in AttackCarryingBuffs)
        {
            buff.BuffUpdate();
            if (buff.Over)
            {
                StateUIMgr.Instance.RemoveBuff(buff);
                AttackCarryingBuffs.Remove(buff);
            }
        }

        // stoic 霸体  移除所有负面状态
        if (isStoic)
        {

        }
    }

    public void BuffOver()
    {

    }

    #region Attack Carrying buffs

    public void RemoveAttackCarryingBuff(IBuff _buff)
    {
       foreach(IBuff buff in AttackCarryingBuffs)
        {
            if(buff.getBuffType() == _buff.getBuffType())
            {
                AttackCarryingBuffs.Remove(buff);
            }
        }
    }

    public void AddAttackCarryingBuff(IBuff _buff)
    {
        foreach (IBuff buff in AttackCarryingBuffs)
        {
            if (buff.getBuffType() == _buff.getBuffType())
            {
                return;
            }
        }
        AttackCarryingBuffs.Add(_buff);
        //StateUIMgr.Instance.AddBuff(buff);
        OnAttackCarryingBuffAdd.Invoke(_buff);
    }

    public bool ContainsAttackCarryingBuff(IBuff buff)
    {
        return AttackCarryingBuffs.Contains(buff);
    }

    public IBuff FindAttackCarryingBuff(IBuff buff)
    {
        foreach (IBuff b in AttackCarryingBuffs)
        {
            if (b.buffID == buff.buffID)
            {
                //b.FlushBuff(buff.buffNum, buff.buffPercentage);
                //b.FlushTime(buff.liveTime);
                return b;
            }
        }
        return null;
    }
    #endregion

    #region Status Buff
    public void AddStatusBuff(IBuff buff)
    {
        StatusBuffs.Add(buff);
        //StateUIMgr.Instance.AddBuff(buff);
        OnStatusBuffAdd.Invoke(buff);
    }

    public bool ContainsStatusBuff(IBuff buff)
    {
        return StatusBuffs.Contains(buff);
    }

    public IBuff FindStatusBuff(IBuff buff)
    {
        foreach (IBuff b in StatusBuffs)
        {
            if (b.buffID == buff.buffID)
            {
                //b.FlushBuff(buff.buffNum, buff.buffPercentage);
                //b.FlushTime(buff.liveTime);
                return b;
            }
        }
        return null;
    }
    #endregion


}


