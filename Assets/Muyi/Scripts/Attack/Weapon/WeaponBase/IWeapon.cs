using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeapon
{
    public int ID;
    public IBuff buff;
    public Sprite sprite;
    public abstract void Init();
    public abstract void Attack(Transform transform, Vector2 vec, List<IBuff> _buff = null);
    public abstract void Update();

    public abstract WeaponType getWeaponType();
    public abstract WeaponName GetWeaponName();
}

public enum WeaponName
{
    /// <summary>
    /// 手枪
    /// </summary>
    PistolGun,
    /// <summary>
    /// 激光枪
    /// </summary>
    SniperGun,
    /// <summary>
    /// 冲锋枪
    /// </summary>
    SubmachineGun,
    /// <summary>
    /// 散弹枪
    /// </summary>
    Shotgun,
    Sword
}


