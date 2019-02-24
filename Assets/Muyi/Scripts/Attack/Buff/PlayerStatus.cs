using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    public float HP = 100;
    public float DamageInfluences = 1; // 百分比

    // 当前身上的buff
    protected List<IBuff> getBuffs = new List<IBuff>();

    private void Update()
    {
        for (int i = 0; i < getBuffs.Count; i++)
        {
            getBuffs[i].BuffUpdate();
            if (getBuffs[i].Over)
            {
                StateUIMgr.Instance.RemoveBuff(getBuffs[i]);
                getBuffs.Remove(getBuffs[i]);
            }
        }
    }

    public void AddBuff(IBuff buff)
    {
        getBuffs.Add(buff);
        StateUIMgr.Instance.AddBuff(buff);
    }

    public bool ContainsBuff(IBuff buff)
    {
        return getBuffs.Contains(buff);
    }

    public IBuff FindBuff(IBuff buff)
    {
        foreach (IBuff b in getBuffs)
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
}


