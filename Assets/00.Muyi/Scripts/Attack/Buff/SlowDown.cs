using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class SlowDown : IBuff<PlayerCharacter>
{
    public SlowDown() : base() { buffID = 2; }
    public SlowDown(float _buffNum, float percnetage = 0) : base( _buffNum, percnetage) { }
    float initalSpeed = 0;

    public void init()
    {
        liveTime = 2f;
        initalSpeed = TClass.maxSpeed;
        buffPercentage = -0.5f;
        buffNum = -7;
    }

    public override void BuffOnEnter(GameObject t)
    {
        TClass = t.GetComponent<PlayerCharacter>();
        init();
        if (buffNum != 0)
        {
            TClass.maxSpeed += buffNum;
        }
        else if (buffPercentage != 0)
        {
            TClass.maxSpeed *= (1 + buffPercentage);
        }
    }

    public override void BuffOver()
    {
        TClass.maxSpeed = initalSpeed;
        Over = true;
    }

    public override void BuffUpdate()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= liveTime)
        {
            BuffOver();
        }
    }

    public override string Des()
    {
        throw new System.NotImplementedException();
    }

    public override void FlushTime(float _time)
    {
        nowTime = 0;
    }

    public override BuffType getBuffType()
    {
        return BuffType.SlowDown;
    }
}



