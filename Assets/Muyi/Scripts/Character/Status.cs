using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {
    public float MaxHP = 100;
    public float HP = 100;
    public float DamageInfluences = 1; // 百分比
    protected float m_AttackDamageNum; // 攻击力

    public float AttackDamageNum
    {
        get { return m_AttackDamageNum * DamageInfluences; }
    }
    // 当前身上的buff
    protected List<IBuff> StatusBuffs = new List<IBuff>();

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
    }

    #region status buffs
    public void AddBuff(IBuff buff)
    {
        StatusBuffs.Add(buff);
        StateUIMgr.Instance.AddBuff(buff);
    }

    public bool ContainsBuff(IBuff buff)
    {
        return StatusBuffs.Contains(buff);
    }

    public IBuff FindBuff(IBuff buff)
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


