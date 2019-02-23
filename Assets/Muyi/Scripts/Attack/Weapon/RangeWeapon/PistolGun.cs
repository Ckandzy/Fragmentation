using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PistolGun : RangedWeapon
{
    public override void Init()
    {
        bullet = Resources.Load<GameObject>("Bullet/PistolGunBullet");
        sprite = Resources.Load<Sprite>("GunSprite/PistolGun");
        this.Frequency = 4;
        mustWaitTime = 1 / 4.00f;
        OffsetPoint = new Vector2(0.36f, 0.22f);
    }

    /// <summary>
    /// 攻击 
    /// </summary>
    /// <param name="_transform">生成的位置</param>
    /// <param name="vec">方向</param>
    /// <param name="_buff">携带的buff</param>
    public override void Attack(Transform _transform, Vector2 vec, IBuff[] _buff = null)
    {
        //if (bullet == null) Start();
        if(nowWaitTime >= mustWaitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = MonoBehaviour.Instantiate(bullet);
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            bullet1.GetComponent<TakeDamager>().Buff = buff;
        }
       
    }
}

public class SniperGun : RangedWeapon
{
    public override void Attack(Transform _transform, Vector2 vec, IBuff[] _buff = null)
    {
        nowWaitTime = 0;
        GameObject bullet1 = MonoBehaviour.Instantiate(bullet);
        bullet1.transform.position = _transform.position;
        bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
        bullet1.GetComponent<TakeDamager>().Buff = buff;
    }

    public override void Init()
    {
        sprite = Resources.Load<Sprite>("GunSprite/Gun1");
        bullet = Resources.Load<GameObject>("Bullet/PistolGunBullet");
        this.Frequency = 4;
        mustWaitTime = 1 / 4.00f;
        OffsetPoint = new Vector2(1.76f, 0.19f);
    }
}

public class Sword : MeleeWeapon
{
    public override void Attack(Transform transform, Vector2 vec, IBuff[] _buff = null)
    {
        throw new NotImplementedException();
    }

    public override void Init()
    {
        sprite = Resources.Load<Sprite>("GunSprite/Gun3");
        offset = new Vector2(1.17f, -0.07f);
        size = new Vector2(1.34f, 2.5f);
    }
}


