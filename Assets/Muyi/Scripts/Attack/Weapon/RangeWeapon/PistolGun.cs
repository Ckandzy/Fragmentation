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
        OffsetPoint = new Vector2(0.364f, 0.218f);
        AttackNum = 10;
    }

    /// <summary>
    /// 攻击 
    /// </summary>
    /// <param name="_transform">生成的位置</param>
    /// <param name="vec">方向</param>
    /// <param name="_buff">携带的buff</param>
    public override void Attack(Transform _transform, Vector2 vec, List<IBuff> _buff = null)
    {
        //if (bullet == null) Start();
        if(nowWaitTime >= mustWaitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = MonoBehaviour.Instantiate(bullet);
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<IBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            foreach (IBuff buff in _buff)
            {
                if(buff  != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
            }
        }
       
    }

    public override void Attack(GameObject _bullet, Transform _transform, Vector2 vec, List<IBuff> _buff = null)
    {
        if (nowWaitTime >= mustWaitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = _bullet;
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            foreach (IBuff buff in _buff)
            {
                if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
            }
        }
    }
}

public class SniperGun : RangedWeapon
{
    public override void Attack(Transform _transform, Vector2 vec, List<IBuff> _buff = null)
    {
        nowWaitTime = 0;
        GameObject bullet1 = MonoBehaviour.Instantiate(bullet);
        bullet1.transform.position = _transform.position;
        bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
        foreach (IBuff buff in _buff)
        {
            if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
        }
    }

    public override void Attack(GameObject _bullet, Transform _transform, Vector2 vec, List<IBuff> _buff = null)
    {
        if (nowWaitTime >= mustWaitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = _bullet;
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            foreach (IBuff buff in _buff)
            {
                if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
            }
        }
    }

    public override void Init()
    {
        sprite = Resources.Load<Sprite>("GunSprite/Gun1");
        bullet = Resources.Load<GameObject>("Bullet/FortBullet");//PistolGunBullet");
        this.Frequency = 4;
        mustWaitTime = 1 / 4.00f;
        OffsetPoint = new Vector2(1.76f, 0.19f);
    }
}

public class Sword : MeleeWeapon
{
    public override void Attack(Transform transform, Vector2 vec, List<IBuff> _buff = null)
    {
        throw new NotImplementedException();
    }

    public override void Init()
    {
        sprite = Resources.Load<Sprite>("GunSprite/Gun3");
        offset = new Vector2(1.09f, 1.3f);
        size = new Vector2(1.24f, 2.83f);
        AttackNum = 1;
        HitPoint = new Vector2(1.32f, 0.01f);
    }
}


