using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragments : MonoBehaviour {
    public FragmentName FragName;
    public Sprite sprite;
    public int Lv = 1;
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
        buffs.Add(BuffFactory.GetBuff((int)BuffType.DamageReduction, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.SlowDown, LV, true));
    }

    public override string Des()
    {
        return "守护者碎片，准绳， 降低25%所受伤害，移速下降30 % （+ 审判 获得技能 铁腕强权）";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.CriterionFrag;
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
        //BuffIds.Add((int)BuffType.AttackMakeBlowout);
        //BuffIds.Add((int)BuffType.SlowDown);
        buffs.Add(BuffFactory.GetBuff((int)BuffType.AttackMakeBlowout, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.GetTwoSegmentJump, LV, true));
    }

    public override string Des()
    {
        return "守护者碎片，震慑碎片， 攻击具有小范围爆炸效果,攻击力下降30 %";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.ShockFrag;
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
        buffs.Add(BuffFactory.GetBuff((int)BuffType.AttackNumUp, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.UpSpikeRate, LV, true));
    }

    public override string Des()
    {
        return "守护者碎片，审判，提高攻击力，并获得10%的秒杀几率（+ 准绳 获得技能 铁腕强权）";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.TrialFrag;
    }
}

/// <summary>
/// 狂躁，移速增加30%， 获得二段跳，血量降低25%
/// </summary>
public class ArrogantFrag : IDemonFrag
{
    public ArrogantFrag()
    {
        buffs.Add(BuffFactory.GetBuff((int)BuffType.GetTwoSegmentJump, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.HPDown, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.UpSpeed, LV, true));
    }

    public override string Des()
    {
        return "移速增加，获得二段跳，最大血量值降低";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.Arrogant;
    }
}

/// <summary>
/// 无序碎片
/// </summary>
public class DisorderFrag : IDemonFrag
{
    public DisorderFrag()
    {
        buffs.Add(BuffFactory.GetBuff((int)BuffType.AtttakMakeCatapult, LV, true));
    }
    public override string Des()
    {
        return "恶魔碎片，无序，攻击会形成一道雷电，随机弹射到距离范围内的敌人身上 （+ 狂暴 获得技能 无法无天）";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.Disorder;
    }
}

/// <summary>
/// 暴乱，伤害提高20%， 获得吸血30%， 最高生命值降低一半，受到的伤害增加20%
/// </summary>
public class RiotFrag : IDemonFrag
{
    public RiotFrag()
    {
        buffs.Add(BuffFactory.GetBuff((int)BuffType.AttackNumUp, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.Bloodsucking, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.HPDown, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.DamageableUp, LV, true));
    }
    public override string Des()
    {
        return "恶魔碎片，攻击力提升， 获取吸血效果， 但最大生命值下降， 受到的伤害提高 （+ 无序 获得技能 无法无天）";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.Riot;
    }
}

/// <summary>
/// 风  移动速度增加，  控制时间减半, 提高闪避率， 获得二段跳， 血量上限降低
/// </summary>
public class WindFrag : INaturalFrag
{
    public WindFrag()
    {
        buffs.Add(BuffFactory.GetBuff((int)BuffType.UpSpeed, 2, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.UpSpikeRate, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.GetTwoSegmentJump, LV, true));
        buffs.Add(BuffFactory.GetBuff((int)BuffType.HPDown, 2, true)); // 50%
    }
    public override string Des()
    {
        return "元碎片，风，移动速度增加，  控制时间减半, 提高闪避率， 获得二段跳， 血量上限降低 （+ 火 获得技能 风助火威）";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.WindFrag;
    }
}

/// <summary>
/// 火， 攻击具有灼烧效果
/// </summary>
public class FireFrag: INaturalFrag
{
    public FireFrag()
    {
        buffs.Add(BuffFactory.GetBuff((int)BuffType.AttackMakeBurning, LV, true));
    }
    public override string Des()
    {
        return "元碎片，火，攻击带有灼烧效果 (+  元碎片 风 获得技能 风助火威)";
    }

    public override FragmentName GetFragName()
    {
        return FragmentName.FireFrag;
    }
}