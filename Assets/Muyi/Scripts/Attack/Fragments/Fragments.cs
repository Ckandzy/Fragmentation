﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragments : MonoBehaviour {
    public FragmentType type;
    public int fragmentID;
    public Sprite sprite;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }
}

/// <summary>
/// 准绳碎片 -- 可以考虑用文件来保存
/// </summary>
public class CriterionFrag : IGuardFrag
{
    public CriterionFrag()
    {
        ID = 1;
        BuffIds.Add((int)BuffType.DamageReduction);
        BuffIds.Add((int)BuffType.SlowDown);
    }

    public override string Des()
    {
        return "守护者碎片，准绳， 降低25%所受伤害，移速下降30 %";
    }
}

/// <summary>
/// 震慑碎片 -- 修改
/// </summary>
public class ShockFrag : IGuardFrag
{
    public ShockFrag()
    {
        ID = 2;
        BuffIds.Add((int)BuffType.DamageReduction);
        BuffIds.Add((int)BuffType.SlowDown);
    }

    public override string Des()
    {
        return "守护者碎片，震慑碎片， 攻击具有小范围爆炸效果,攻击力下降30 %";
    }
}

/// <summary>
/// 审判 -- 修改
/// </summary>
public class TrialFrag : IGuardFrag
{
    public TrialFrag()
    {
        ID = 3;
        BuffIds.Add((int)BuffType.DamageReduction);
        BuffIds.Add((int)BuffType.SlowDown);
    }

    public override string Des()
    {
        return "守护者碎片，准绳， 降低25%所受伤害，移速下降30 %";
    }
}