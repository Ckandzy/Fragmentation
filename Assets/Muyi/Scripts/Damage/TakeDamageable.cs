﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
using UnityEngine.Events;
using System;


[RequireComponent(typeof(Status))]
public class TakeDamageable : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class HealthEvent : UnityEvent<TakeDamageable>
    { }

    /// <summary>
    /// 伤害事件
    /// </summary>
    [Serializable]
    public class DamageEvent : UnityEvent<TakeDamager, TakeDamageable>
    { }

    /// <summary>
    /// 治愈事件
    /// </summary>
    [Serializable]
    public class HealEvent : UnityEvent<int, TakeDamageable>
    { }

    Status Status;

    [Tooltip("当收到伤害时无敌")]
    public bool invulnerableAfterDamage = true;
    [Tooltip("无敌效果持续时间")]
    public float invulnerabilityDuration = 3f;
    public bool disableOnDeath = false;
    [Tooltip("An offset from the object position used to set from where the distance to the damager is computed")]
    public Vector2 centreOffset = new Vector2(0f, 1f);
    public HealthEvent OnHealthSet;
    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;
    public HealEvent OnGainHealth;
    [HideInInspector]
    public DataSettings dataSettings;

    protected bool m_Invulnerable;
    protected float m_InulnerabilityTimer;
    protected Vector2 m_DamageDirection;
    protected bool m_ResetHealthOnSceneReload;

    public float CurrentHealth
    {
        get { return Status.HP; }
    }

    private void Start()
    {
        Status = GetComponent<Status>();
    }


    void OnEnable()
    {
        //PersistentDataManager.RegisterPersister(this);
        //m_CurrentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        DisableInvulnerability();
    }

    void OnDisable()
    {
        //PersistentDataManager.UnregisterPersister(this);
    }

    void Update()
    {
        if (m_Invulnerable)
        {
            m_InulnerabilityTimer -= Time.deltaTime;

            if (m_InulnerabilityTimer <= 0f)
            {
                m_Invulnerable = false;
            }
        }
    }

    /// <summary>
    /// 开启无敌
    /// </summary>
    /// <param 是否忽略无敌时间="ignoreTimer"></param>
    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        //technically don't ignore timer, just set it to an insanly big number. Allow to avoid to add more test & special case.
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    /// <summary>
    /// 关闭无敌
    /// </summary>
    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
    }

    public Vector2 GetDamageDirection()
    {
        return m_DamageDirection;
    }



    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="ignoreInvincible"></param>
    public void TakeDamage(TakeDamager damager, List<IBuff> buffs = null, bool ignoreInvincible = false)
    {
        // 无敌，或忽略伤害
        if ((m_Invulnerable && !ignoreInvincible) || CurrentHealth <= 0)
            return;

        //we can reach that point if the damager was one that was ignoring invincible state.
        //We still want the callback that we were hit, but not the damage to be removed from health.
        if (!m_Invulnerable)
        {
            // 如果身上带有buff， 则刷新buff时间持续时间和效果
            foreach (IBuff buff in buffs)
            {
                if (buff != null)
                {

                    if (!Status.ContainsBuff(buff))
                    {
                        Status.AddBuff(buff);
                        buff.BuffOnEnter(gameObject);
                    }
                    else
                    {
                        IBuff b = Status.FindBuff(buff);
                        if (b != null)
                        {
                            b.FlushBuff(buff.buffNum, buff.buffPercentage);
                            b.FlushTime(buff.liveTime);
                        }
                    }
                }
            }

            Status.HP -= damager.DamageNum * Status.DamageInfluences;

            OnHealthSet.Invoke(this);
        }

        m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

        OnTakeDamage.Invoke(damager, this);

        if (Status.HP <= 0)
        {
            OnDie.Invoke(damager, this);
            //m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
    }
}


