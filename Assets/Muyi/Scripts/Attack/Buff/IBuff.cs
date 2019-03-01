﻿using System.Collections;
using System.Collections.Generic;
using Gamekit2D;
using UnityEngine;

[System.Serializable]
public abstract class IBuff
{
    public IBuff() { LV = 1; }
    public IBuff(int _lv) { LV = _lv; }
    public int buffID; // 用于识别buff，工厂模式或其他可用
    /// <summary>
    /// 持续时间
    /// </summary>
    protected float liveTime = 0.5f; // 持续时间
    /// <summary>
    /// 当前已经持续的时间
    /// </summary>
    protected float nowTime = 0; // 当前已经持续的时间
    /// <summary>
    /// buff的数值，如减速多少，每秒灼烧多少等
    /// </summary>
    protected float buffNum = 0;
    /// <summary>
    /// 百分比，如减伤25%。
    /// </summary>
    [Range(0, 1)]
    protected float buffPercentage = 0;
    /// <summary>
    /// 永久
    /// </summary>
    protected bool permanent = false;
    /// <summary>
    /// buff的等级--持续时间等由等级来算
    /// </summary>
    public int LV = 1;
    public bool Over = false;
    public abstract string Des();
    public abstract void BuffOnEnter(GameObject obj);
    public abstract void BuffOver();

    public virtual void FlushTime(float _livetime) { nowTime = 0; liveTime = _livetime; }
    public virtual void FlushTime(bool isPermanent) { permanent = isPermanent; }
    public virtual void FlushLV(int lv) { LV = lv; }

    public virtual void BuffUpdate()
    {
        if(!permanent)
            nowTime += Time.deltaTime;
        if (nowTime >= liveTime) Over = true;
    }


    public abstract void CalculationBuffNum();
    public abstract BuffType getBuffType();
    public abstract BuffEffectType getBuffEffectType();
}

public abstract class IBuff<T> : IBuff
{
    protected T TClass;
    public IBuff(int lv):base(lv) { } // 使用lv来控制BuffNum 和 buffpercentage
    public IBuff() : base() { }
}

public abstract class GainBuff<T> : IBuff<T>
{
    public GainBuff(int lv):base(lv) { } // 使用lv来控制BuffNum 和 buffpercentage

    public GainBuff() : base() { }

    public override BuffEffectType getBuffEffectType()
    {
        return BuffEffectType.Gain;
    }
}

public abstract class NegativeBuff<T> : IBuff<T>
{
    public NegativeBuff(int lv):base(lv) { } // 使用lv来控制BuffNum 和 buffpercentage

    public NegativeBuff() : base() { }

    public override BuffEffectType getBuffEffectType()
    {
        return BuffEffectType.Negative;
    }
}


public abstract class AttackTakeBuff<T> : IBuff<T>
{
    public IBuff TakeBuff;

    public AttackTakeBuff(int lv):base(lv) {  } // 使用lv来控制BuffNum 和 buffpercentage

    public AttackTakeBuff() : base() { }

    public override BuffEffectType getBuffEffectType()
    {
        return BuffEffectType.Gain;
    }
}
/*
// 定身
// 建议人物和敌人的控制函数继承Controller函数，Controller函数中有速度，等基础类型，及函数
// T ：Controller
// CharacterController2D里面写一个定身函数， 这里直接调用，目前没写，暂时用MonoBehaviour
// 这里是禁止按钮调用--只能定身人物
public class SetUpBuff : IBuff
{
    public SetUpBuff() : base() { buffID = 1; }
    public override string Des()
    {
        // 定身 n 秒
        return "定身" + buffNum + "秒";
    }

    public override void BuffOnEnter()
    {
        liveTime = 1f;
        Gamekit2D.PlayerInput.Instance.ReleaseControl(true);
        StateUIMgr.Instance.AddState(StateUIMgr.SetUpStateItem);
    }

    public override void BuffOver()
    {
        Gamekit2D.PlayerInput.Instance.GainControl();
        Over = true;
        StateUIMgr.Instance.RemoveState(StateUIMgr.SetUpStateItem);
    }

    public override void BuffUpdate()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= liveTime) BuffOver();
    }

    public override void FlushTime(float _time)
    {
        liveTime = _time;
        nowTime = 0;
    }
}
// 灼烧
// 添加泛型且应该为人物的属性
public class BurningBuff : IBuff
{
    //public BurningBuff(CharacterController2D t) : base(t) { }
    public BurningBuff() : base() { buffID = 2; }
    public override void BuffOnEnter()
    {
        liveTime = 2;
    }

    public override void BuffOver()
    {
        Over = true;
        StateUIMgr.Instance.RemoveState(StateUIMgr.SetUpStateItem);
    }

    public override void BuffUpdate()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= liveTime) BuffOver();
    }

    public override string Des()
    {
        // 每秒灼烧n伤害
        return "每秒灼烧" + buffNum + "伤害";
    }

    public override void FlushTime(float _time)
    {
        liveTime = _time;
        nowTime = 0;
    }

}
*/

