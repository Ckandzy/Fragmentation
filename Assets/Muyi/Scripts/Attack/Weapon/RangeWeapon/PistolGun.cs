using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 手枪
/// </summary>
public class PistolGun : RangedWeapon
{
    public override void Init()
    {
        bullet = Resources.Load<GameObject>("Bullet/PistolGunBullet");
        sprite = Resources.Load<Sprite>("GunSprite/PistolGun");
        this.Frequency = 4;
        mustWaitTime = 1 / 4.00f;
        OffsetPoint = new Vector2(0.32f, 0.19f);
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

    public override WeaponName GetWeaponName()
    {
        return WeaponName.PistolGun;
    }
}
/// <summary>
/// 激光枪
/// </summary>
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
            //Vector2 start = _transform.position;
            //Vector2 end = (Vector2)_transform.position + new Vector2(30 * _transform.lossyScale.x, 0);
            //VFXControllerM.Instance.MakeLaser(start, end, 0.2f);
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

    public override WeaponName GetWeaponName()
    {
        return WeaponName.SniperGun;
    }

    public override void Init()
    {
        sprite = Resources.Load<Sprite>("GunSprite/Gun1");
        bullet = Resources.Load<GameObject>("Bullet/ShotGunBullet");//PistolGunBullet");
        this.Frequency = 4;
        mustWaitTime = 1 / 4.00f;
        OffsetPoint = new Vector2(1.76f, 0.19f);
    }
}

/// <summary>
/// 冲锋枪
/// </summary>
public class SubmachineGun : RangedWeapon
{
    public override void Init()
    {
        bullet = Resources.Load<GameObject>("Bullet/SubmachineGunBullet");
        sprite = Resources.Load<Sprite>("GunSprite/SubmachineGun");
        this.Frequency = 4;
        mustWaitTime = 1 / 4.00f;
        OffsetPoint = new Vector2(0.37f, 0.216f);
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
        if (nowWaitTime >= mustWaitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = MonoBehaviour.Instantiate(bullet);
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<IBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            foreach (IBuff buff in _buff)
            {
                if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
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

    public override WeaponName GetWeaponName()
    {
        return WeaponName.SubmachineGun;
    }

    public IEnumerator Shoot(Status status, ABulletsPool _bullet, Transform _transform, Vector2 vec, List<IBuff> _buff = null)
    {
        if (nowWaitTime >= mustWaitTime)
            for (int i = 0; i < 5; i++)
            {
            nowWaitTime = 0;
            GameObject bullet1 = _bullet.Pop().transform.gameObject;
            status.RegisteredTakeDamger(bullet1.GetComponent<TakeDamager>());
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            foreach (IBuff buff in _buff)
            {
                if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
            }
            yield return new WaitForSeconds(0.05f);
            }
    }
}

public class Shotgun : RangedWeapon
{
    public override void Init()
    {
        bullet = Resources.Load<GameObject>("Bullet/ShotGunBullet");
        sprite = Resources.Load<Sprite>("GunSprite/shotgun");
        this.Frequency = 4;
        mustWaitTime = 1 / 3.00f; // 一秒1次 
        OffsetPoint = new Vector2(0.813f, 0.188f);
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
        if (nowWaitTime >= mustWaitTime)
        {
            nowWaitTime = 0;
            GameObject bullet1 = MonoBehaviour.Instantiate(bullet);
            bullet1.transform.position = _transform.position;
            bullet1.GetComponent<IBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
            foreach (IBuff buff in _buff)
            {
                if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
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

    public override WeaponName GetWeaponName()
    {
        return WeaponName.Shotgun;
    }

    public IEnumerator Shoot(Status status, ABulletsPool _bullet, Transform _transform, Vector2 vec, List<IBuff> _buff = null)
    {
        float num = 6;
        float angle = 70;
        if (nowWaitTime >= mustWaitTime)
            for (int i = 0; i < num; i++)
            {
                nowWaitTime = 0;
                GameObject bullet1 = _bullet.Pop().transform.gameObject;
                status.RegisteredTakeDamger(bullet1.GetComponent<TakeDamager>());
                bullet1.transform.position = _transform.position;
                bullet1.transform.eulerAngles = new Vector3(0, 0, angle * num - num / 2 * angle);
                float y = Mathf.Deg2Rad * Mathf.Atan(1.0f / (i - num / 2.0f) * angle) * 10;
                vec = new Vector2(1, y);// y/x = angle
                bullet1.GetComponent<RangeWeaponBullet>().Set(bulletLiveTime, bulletSpeed, vec, null);
                foreach (IBuff buff in _buff)
                {
                    if (buff != null) bullet1.GetComponent<TakeDamager>().TakeAttackBuffs.Add(buff);
                }
                yield return new WaitForSeconds(0);
            }
    }
}

public class Sword : MeleeWeapon
{
    public override void Attack(Transform transform, Vector2 vec, List<IBuff> _buff = null)
    {
        throw new NotImplementedException();
    }

    public override WeaponName GetWeaponName()
    {
        return WeaponName.Sword;
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


