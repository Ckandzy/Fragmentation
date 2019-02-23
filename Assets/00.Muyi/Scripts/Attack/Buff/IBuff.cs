using System.Collections;
using System.Collections.Generic;
using Gamekit2D;
using UnityEngine;

public abstract class IBuff
{
    public IBuff(float _buffNum, float percnetage = 0)
    {
        buffNum = _buffNum;
        buffPercentage = percnetage;
    }
    public IBuff() { }
    public int buffID; // 用于识别buff，工厂模式或其他可用
    /// <summary>
    /// 持续时间
    /// </summary>
    public float liveTime = 0.5f; // 持续时间
    /// <summary>
    /// 当前已经持续的时间
    /// </summary>
    public float nowTime = 0; // 当前已经持续的时间
    /// <summary>
    /// buff的数值，如减速多少，每秒灼烧多少等
    /// </summary>
    public float buffNum = 0;
    /// <summary>
    /// 百分比，如减伤25%。
    /// </summary>
    [Range(0, 1)]
    public float buffPercentage = 0;
    /// <summary>
    /// buff的等级--持续时间等由等级来算
    /// </summary>
    public int LV = 1;
    public bool Over = false;
    public abstract string Des();
    public abstract void BuffOnEnter(GameObject obj);
    public abstract void BuffUpdate();
    public abstract void BuffOver();
    public abstract void FlushTime(float _time);
    public virtual void FlushBuff(float _num, float _percentage = 0)
    {
        buffNum = _num;
        buffPercentage = _percentage;
    }
    public abstract BuffType getBuffType();
}

public abstract class IBuff<T> : IBuff
{
    public T TClass;

    public IBuff(int lv) { this.LV = lv; } // 使用lv来控制BuffNum 和 buffpercentage

    public IBuff(float _buffNum, float percnetage = 0)
    {
        buffNum = _buffNum;
        buffPercentage = percnetage;
    }
    public IBuff() : base() { }
    //public IBuff(float _buffNum, float percnetage = 0) : base(_buffNum, percnetage) { }
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

