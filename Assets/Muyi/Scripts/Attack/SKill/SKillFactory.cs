using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKillFactory : MonoBehaviour
{
    public static SkillBase GetSkill(int id)
    {
        switch (id)
        {
            case 1: return new UnlawlessSkill();
            case 2: return new IronFistPowerSKill();
        }
        throw new System.Exception("no skill but try get it");
    }
}
