using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
using UnityEngine.Events;
using System;

public class TakeDamager : MonoBehaviour {
    [Serializable]
    public class DamagableEvent : UnityEvent<TakeDamager, TakeDamageable>{ }
   
    [Serializable]
    public class NonDamagableEvent : UnityEvent<TakeDamager>{ }
    [Serializable]
    public class OnHitMissingEvent : UnityEvent { } 

    public DamageType ADamageType = DamageType.Once;
    public Status status;  // 面板拖拽 or use Status' RegisteredTakeDamger(this) way
    public List<IBuff> TakeAttackBuffs { get {return status.AttackCarryingBuffs; } }
    public float CurrentDamagNum { get { return status.AttackDamageNum * status.TakeDamageInfluences; } }
    public float SpikeRate { get { return status.SpikeRate; } }
    //call that from inside the onDamageableHIt or OnNonDamageableHit to get what was hit.
    public Collider2D LastHit { get { return m_LastHit; } }

    [Tooltip("攻击范围指示框的偏移量")]
    public Vector2 offset = new Vector2(1.5f, 1f);
    [Tooltip("攻击范围指示框的大小")]
    public Vector2 size = new Vector2(2.5f, 1f);
    [Tooltip("对同一个物体造成伤害后，n秒后且再次处于碰撞中才可以造成伤害")]
    public float RefreshDamageTime = 0.5f;
    /// <summary>
    /// 根据Sprite翻转调整偏移方向(正负)
    /// </summary>
    [Tooltip("If this is set, the offset x will be changed base on the sprite flipX setting. e.g. Allow to make the damager alway forward in the direction of sprite")]
    public bool offsetBasedOnSpriteFacing = true;
    [Tooltip("SpriteRenderer used to read the flipX value used by offsetBasedOnSpriteFacing")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("If disabled, damager ignore trigger when casting for damage 如果关闭，投掷攻击将会忽略触发器")]
    public bool canHitTriggers;
    public bool disableDamageAfterHit = false;
    //be forced to 被迫。。。 in addition to 除了。。。
    [Tooltip("If set, the player will be forced to respawn to latest checkpoint in addition to loosing life")]
    public bool forceRespawn = false;
    [Tooltip("If set, an invincible damageable hit will still get the onHit message (but won't lose any life) 如果开启，无敌状态的被攻击对象仍然能接收到onHit消息，但不会损失生命")]
    public bool ignoreInvincibility = false;
    public LayerMask hittableLayers;
    public DamagableEvent OnDamageableHit;
    public NonDamagableEvent OnNonDamageableHit;
    public OnHitMissingEvent OnHitMissing;
    //public ContactPoint2D[] AttackPoints = new ContactPoint2D[10];
    public int HitMaxNum = 10;
    public Vector2 hitPoint = Vector2.zero;
    //Sprite初始翻转状态
    protected bool m_SpriteOriginallyFlipped;
    protected bool m_CanDamage = true;

    protected ContactFilter2D m_AttackContactFilter;
    protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
    protected Transform m_DamagerTransform;
    protected Collider2D m_LastHit;

    private void OnEnable()
    {
        if(hitTransforms != null)
            foreach(Transform trans in hitTransforms)
            {
                RemoveHitInfo(trans);
            }
    }

    void Awake()
    {
        m_AttackContactFilter.layerMask = hittableLayers;
        m_AttackContactFilter.useLayerMask = true;
        m_AttackContactFilter.useTriggers = canHitTriggers;

        if (offsetBasedOnSpriteFacing && spriteRenderer != null)
            m_SpriteOriginallyFlipped = spriteRenderer.flipX;

        m_DamagerTransform = transform;

        if(status == null)
            status = GetComponent<Status>();
    }

    private void Start()
    {
        hitTransforms = new Transform[HitMaxNum];
        hitTimersCounter = new float[HitMaxNum];
        m_AttackOverlapResults = new Collider2D[HitMaxNum];
        if(ADamageType == DamageType.Once)
        {
            //OnHitMissing.AddListener();
        }
    }

    /// <summary>
    /// 开启攻击
    /// </summary>
    public void EnableDamage()
    {
        m_CanDamage = true;
    }

    /// <summary>
    /// 关闭攻击
    /// </summary>
    public void DisableDamage()
    {
        m_CanDamage = false;
    }
    
    Transform[] hitTransforms;
    float[] hitTimersCounter;
    
    // 
    Vector2 LastFramePoint = Vector2.zero;
    Vector2 CurrentFramePoint = Vector2.right;

    void FixedUpdate()
    {
        Vector2 scale = m_DamagerTransform.lossyScale;
        Vector2 facingOffset = Vector2.Scale(offset, scale);

        CurrentFramePoint = (Vector2)transform.position + facingOffset;
        float len = 10;
        if (!m_CanDamage)
            return;

        //if (offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.flipX != m_SpriteOriginallyFlipped)
        //    facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

        Vector2 scaledSize = Vector2.Scale(size, scale);

        //点A,B分别是攻击范围指示框的左下和右上点
        Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;

        
        int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            Transform trans = m_AttackOverlapResults[i].transform;
            if (!IsCanHit(trans)) continue;
            AddHitInfo(trans);

            m_LastHit = m_AttackOverlapResults[i];
            //m_LastHit.GetContacts(AttackPoints); // 这个方法获得碰撞点始终有误差
            
            TakeDamageable damageable = m_LastHit.GetComponent<TakeDamageable>();
            // hit is Missing
            if (damageable && !damageable.isHit(damageable.DodgeRate))
            {
                OnHitMissing.Invoke();
                damageable.OnMissingAttack.Invoke();
            }

            if (damageable)
            {
                hitPoint = getPoint(CurrentFramePoint, (CurrentFramePoint - LastFramePoint).normalized, len, hittableLayers);
                damageable.TakeDamage(this, TakeAttackBuffs, ignoreInvincibility);
                OnDamageableHit.Invoke(this, damageable);
                if (disableDamageAfterHit)
                    DisableDamage();
            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }
        }
    }

    private void LateUpdate()
    {
        FlushHitInfo();
        if(LastFramePoint != CurrentFramePoint)
            LastFramePoint = CurrentFramePoint;
    }

    /// <summary>
    /// when damagetype is once, check the transfrom had hit
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns></returns>
    public bool IsContainsHitTransform(Transform _transform)
    {
        foreach(Transform trans in hitTransforms)
        {
            if (trans == _transform) return true;
        }
        return false;
    }

    public bool IsCanHit(Transform _trans)
    {
        if (!IsContainsHitTransform(_trans)) return true;
  
        for (int i = 0; i < HitMaxNum; i++)
        {
            if (_trans == hitTransforms[i] && hitTimersCounter[i] >= RefreshDamageTime) return true;
        }

        return false;
    }

    private void AddHitInfo(Transform _trans)
    {
        int i = 0;
        for (i = 0; i < HitMaxNum; i++)
        {
            if (hitTransforms[i] == null)
            {
                hitTransforms[i] = _trans;
                hitTimersCounter[i] = 0;
                return;
            }
        }
    }
    /*
    private Vector2 GetHitPoint()
    {
        Vector2 scale = transform.localScale;
        Vector2 facingOffset = Vector2.Scale(offset, scale);

        Vector2 scaledSize = Vector2.Scale(size, scale);

        //点A,B分别是攻击范围指示框的左下和右上点
        Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;
        Vector2 start = new Vector2((pointB.x - pointA.x) * 0.5f + pointA.x, (pointB.y - pointA.y) * 0.5f + pointA.y);
        Vector2 end = start + new Vector2(scale.x * (pointB.x - pointA.x) * 0.5f, 0);
        RaycastHit2D hit = Physics2D.Linecast(start, end, hittableLayers);
        Debug.DrawLine(start, Vector2.zero);
        Debug.DrawLine(pointA, pointB);
        if(hit.transform != null)
        {
            return hit.point;
        }
        // 如果位置正好是zero--待解决
        return Vector2.zero;
    }
    */
    private Vector2 getPoint(Vector2 start, Vector2 dir, float len, LayerMask layer)
    {
        RaycastHit2D hit =  Physics2D.Raycast(start, dir, len, layer);
        Debug.Log(dir + transform.name);
        Debug.DrawRay(start, dir, Color.blue);
        return hit.point;
    }

    private void RemoveHitInfo(Transform _trans)
    {
        for (int i = 0; i < HitMaxNum; i++)
        {
            if (hitTransforms[i] == _trans)
            {
                hitTransforms[i] = null;
                hitTimersCounter[i] = 0;
                return;
            }
            
        }
    }

    private void FlushHitInfo()
    {
        for (int i = 0; i < HitMaxNum; i++)
        {
            hitTimersCounter[i] += Time.deltaTime;
            if(hitTimersCounter[i] > RefreshDamageTime)
            {
                RemoveHitInfo(hitTransforms[i]);
            }
        }
    }
    private void Update()
    {

    }

    private void OnDrawGizmos()
    {

    }

    // 是否秒杀
    public bool IsSpike()
    {
        return UnityEngine.Random.Range(1, 11) / 10.0f < SpikeRate;
    }
}

public enum DamageType
{
    Once,
    Continued
}

   