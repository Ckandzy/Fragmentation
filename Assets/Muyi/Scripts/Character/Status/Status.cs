﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Status : MonoBehaviour {
    [System.Serializable]
    public class BuffAttribute
    {
        public int BuffId;
        public int Lv;
        public bool Forever;
    }

    [System.Serializable]
    public class BuffEvent : UnityEvent<IBuff> { }
    public CharacterType StatusType;
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float hp = 100;
    public float MaxHP { get { return maxHp; } set { maxHp = value; if (HP > maxHp) { HP = maxHp; } } }// 最大生命值
    public float HP {
        get { return hp; }
        set {
            hp = value;
            if (hp > maxHp) hp = maxHp;
            if (hp <= 0) 
                for(int i = 0; i < TakeDamageables.Count; i++)
                {
                    TakeDamageables[i].OnDie.Invoke(null, null);
                }
            }
    }
    
    // all TakeDamagers such bullet, and melee take, you can add to this list
    public List<TakeDamager> TakeDamagers;
    public List<TakeDamageable> TakeDamageables;

    public float HurtInfluences = 1;// influences of hurt such as Damage reduction 20%
    public float SpikeRate = 0; // Spike：秒杀
    public float AttackDamageNum = 1; // Attack num
    public float TakeDamageInfluences = 1; // influences of DamageNum 

    public float Precision = 1; // 精准度
    public float DodgeRate = 0; // 闪避率
    public float BloodsuckingRate = 0f; // 吸血率
    public BuffEvent OnStatusBuffAdd;
    public BuffEvent OnAttackCarryingBuffAdd;
    public BuffEvent OnStatusBuffRemove;
    [Tooltip("初始携带的buff")]
    public List<BuffAttribute> buffAttributes = new List<BuffAttribute>();

    // 当前身上的状态buff -- 如受到减速
    protected List<IBuff> m_StatusBuffs = new List<IBuff>();

    // 当前身上影响攻击的buff -- 如攻击携带减速效果 -- 只能提供影响攻击的状态buff获取或失去这些buff
    protected List<IBuff> m_AttackCarryingBuffs = new List<IBuff>();

    public List<IBuff> StatusBuffs { get { return m_StatusBuffs; } }
    public List<IBuff> AttackCarryingBuffs { get { return m_AttackCarryingBuffs; } }
    
    public bool isStoic = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        for(int i = 0; i < buffAttributes.Count; i++)
        {
            AddStatusBuff(BuffFactory.GetBuff(buffAttributes[i].BuffId, buffAttributes[i].BuffId, buffAttributes[i].Forever));
        }
        foreach (TakeDamager damager in TakeDamagers) { damager.status = this; }
        foreach (TakeDamageable damageable in TakeDamageables) { damageable.status = this; }
    }

    private void Update()
    {
        for (int i = 0; i < m_StatusBuffs.Count; i++)
        {
            m_StatusBuffs[i].BuffUpdate();
            if (m_StatusBuffs[i].Over)
            {
                // StateUIMgr.Instance.RemoveBuff(m_StatusBuffs[i]);
                OnStatusBuffRemove.Invoke(m_StatusBuffs[i]);
                RemoveStatuBuff(m_StatusBuffs[i]);
                
            }
        }

        // stoic 霸体  移除所有负面状态
        if (isStoic)
        {

        }
    }
    #region bind or unbind TakeDamager or TakeDamagable in Statu 
    public void RegisteredTakeDamger(TakeDamager takeDamager)
    {
        if (!TakeDamagers.Contains(takeDamager))
        {
            TakeDamagers.Add(takeDamager);
            takeDamager.status = this;
        }
    }

    public void unRegisteredTakeDamger(TakeDamager takeDamager)
    {
        if (TakeDamagers.Contains(takeDamager))
        {
            TakeDamagers.Remove(takeDamager);
            takeDamager.status = null;
        }
    }

    public void TakeDamagerFlush()
    {
        for(int i = 0;i < TakeDamagers.Count; i++)//foreach (TakeDamager damager in TakeDamagers)
        {
            TakeDamager damager = TakeDamagers[i];
            if (damager == null || damager.gameObject == null)
            {
                unRegisteredTakeDamger(damager);
            }
        }
    }

    public void RegisteredTakeDamagable(TakeDamageable takeDamageable)
    {
        if (TakeDamageables.Contains(takeDamageable))
        {
            takeDamageable.status = this;
            TakeDamageables.Add(takeDamageable);
        }
    }

    public void unRegisteredTakeDamagable(TakeDamageable takeDamageable)
    {
        if (TakeDamageables.Contains(takeDamageable))
        {
            takeDamageable.status = null;
            TakeDamageables.Remove(takeDamageable);
        }
    }

    
    #endregion

    #region Attack Carrying buffs

    public List<IBuff> getAttackBuffs()
    {
        return this.m_AttackCarryingBuffs;
    }

    public void RemoveAttackCarryingBuff(IBuff _buff)
    {
        foreach(IBuff buff in m_AttackCarryingBuffs)
        {
            if(buff == _buff)
            {
                buff.BuffOver();
                m_AttackCarryingBuffs.Remove(buff);
            }
        }
    }

    public void AddAttackCarryingBuff(IBuff _buff)
    {
        if (AttackCarryingBuffs.Contains(_buff))
        {
            m_AttackCarryingBuffs.Add(_buff);
            _buff.BuffOnEnter(gameObject);
            OnAttackCarryingBuffAdd.Invoke(_buff);
        }
        
    }

    public bool ContainsAttackCarryingBuff(IBuff buff)
    {
        return m_AttackCarryingBuffs.Contains(buff);
    }
    #endregion

    #region Status Buff
    //  current if player has type of this buff of his status buffs, add false 
    public void AddStatusBuff(IBuff _buff)
    {
        //if (!StatusBuffs.Contains(_buff))
        {
            m_StatusBuffs.Add(_buff);
            _buff.BuffOnEnter(gameObject);
            OnStatusBuffAdd.Invoke(_buff);
        }
    }

    public void AddStatusBuff(List<IBuff> _buff)
    {
        if (_buff == null) return;
        m_StatusBuffs.AddRange(_buff);
        foreach(IBuff buff in _buff)
        {
            buff.BuffOnEnter(gameObject);
            OnStatusBuffAdd.Invoke(buff);
        }
    }

    public void RemoveStatuBuff(List<IBuff> _buff)
    {
        if (_buff == null) return;
        //m_StatusBuffs.AddRange(_buff);
        foreach (IBuff buff in _buff)
        {
            buff.Over = true;
            buff.BuffOver();
            m_StatusBuffs.Remove(buff);
            OnStatusBuffRemove.Invoke(buff);
        }
    }

    public void RemoveStatuBuff(IBuff _buff)
    {
        if (m_StatusBuffs.Contains(_buff))
        {
            //Debug.Log(StatusBuffs.Count);
            _buff.Over = true;
            _buff.BuffOver();
            m_StatusBuffs.Remove(_buff);
            OnStatusBuffRemove.Invoke(_buff);
        }
    }

    public bool ContainsStatusBuff(IBuff _buff)
    {
        return m_StatusBuffs.Contains(_buff);
    }
    #endregion


}

public enum CharacterType
{
    /// <summary>
    /// 玩家
    /// </summary>
    Player,
    /// <summary>
    /// 杂兵
    /// </summary>
    LowLevelEnemy,
    /// <summary>
    /// 精英
    /// </summary>
    EliteEnemy,
    /// <summary>
    /// boss
    /// </summary>
    Boss
}