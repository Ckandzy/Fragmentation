using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
[RequireComponent(typeof(PlayerStatus))]
public class DamageAble : Damageable {

    //暂时未添加unityEvent

    PlayerStatus Status;

    private void Start()
    {
        Status = GetComponent<PlayerStatus>();
    }

    public void WasDamage(float takeDamageNum, IBuff buff)
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
        
        Status.HP -= takeDamageNum * Status.DamageInfluences;

        if (Status.HP <= 0)
        {
            if (transform.tag != "Player")
                Destroy(gameObject);
        }
        // 受伤动画
    }

    public void TakeDamage()
    {

    }

    public void TakeDamage(TakeDamager damager, IBuff buff = null, bool ignoreInvincible = false)
    {
        if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
            return;
        //we can reach that point if the damager was one that was ignoring invincible state.
        //We still want the callback that we were hit, but not the damage to be removed from health.
        if (!m_Invulnerable)
        {
            m_CurrentHealth -= damager.damage;
            OnHealthSet.Invoke(this);
        }
        m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

        OnTakeDamage.Invoke(damager, this);

        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(damager, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvulnerability();
            if (disableOnDeath) gameObject.SetActive(false);
        }
    }
}


