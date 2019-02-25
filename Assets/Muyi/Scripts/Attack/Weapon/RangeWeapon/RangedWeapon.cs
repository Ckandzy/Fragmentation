using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : IWeapon
{
    public int AttackRange; // 攻击范围
    public int Frequency; // 攻击频率
    public Vector2 OffsetPoint; // 发射点的位置 
    public GameObject bullet;
    public float bulletLiveTime = 2;
    public float bulletSpeed = 10;
    protected float nowWaitTime = 100;
    protected float mustWaitTime = 0;

    public override void Update()
    {
        nowWaitTime += Time.deltaTime;
    }

    public abstract void Attack(GameObject _bullet, Transform transform, Vector2 vec, List<IBuff> _buff = null);

    public override WeaponType getWeaponType()
    {
        return WeaponType.RangeType;
    }
}

public abstract class MeleeWeapon : IWeapon
{
    public Vector2 offset;
    public Vector2 size;
    public override void Update()
    {
        //nowWaitTime += Time.deltaTime;
    }

    public override WeaponType getWeaponType()
    {
        return WeaponType.MeleeType;
    }
}


