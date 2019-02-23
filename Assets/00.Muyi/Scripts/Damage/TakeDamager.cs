using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
using UnityEngine.Events;

public class TakeDamager : Damager {
    public IBuff Buff;

    public class BuffDamagableEvent : UnityEvent<Damager, Damageable, IBuff> { }
    /*
    public void FixedUpdate()
    {
        if (!m_CanDamage)
            return;
        // transform.lossyScale 得到准确的缩放
        Vector2 scale = m_DamagerTransform.lossyScale;

        Vector2 facingOffset = Vector2.Scale(offset, scale);
        if (offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.flipX != m_SpriteOriginallyFlipped)
            //2018.7.24 Hotkang 这里再乘scale可能多余，因为之前已经Vector2.Scale(offset, scale);
            //2019.1.4 Muyi 前面的初始值直接被覆盖掉了，根本没有用
            facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

        Vector2 scaledSize = Vector2.Scale(size, scale);

        //点A,B分别是攻击范围指示框的左下和右上点
        Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;

        int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            m_LastHit = m_AttackOverlapResults[i];
            DamageAble damageable = m_LastHit.GetComponent<DamageAble>();

            if (damageable)
            {
                OnDamageableHit.Invoke(this, damageable);
                damageable.TakeDamage(this, ignoreInvincibility);
                if (disableDamageAfterHit)
                    DisableDamage();
            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }
        }
    } */
}



   