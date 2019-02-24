using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffFactory
{
    public static IBuff GetBuff(int index)
    {
        switch (index)
        {
            case 1: return new DamageReductionBuff();
            case 2: return new SlowDown();
        }
        throw new System.Exception("no this buff, but try get it");
    }
}

public enum BuffType
{
    DamageReduction = 1,
    SlowDown = 2
}


