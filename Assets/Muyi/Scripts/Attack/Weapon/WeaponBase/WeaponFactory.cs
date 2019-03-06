using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponFactory {
    public static IWeapon GetWeapon(WeaponType type, int id)
    {
        switch (type)
        {
            case WeaponType.MeleeType:
                return getMeleeWeapon(id);
            case WeaponType.RangeType:
                return getRangedWeapon(id);
        }
        throw new System.Exception("No weapon, but try get it");
    }

    private static RangedWeapon getRangedWeapon(int id)
    {
        switch (id)
        {
            case 1: return new PistolGun();
            case 2: return new SniperGun();
            case 3: return new SubmachineGun();
            case 4: return new Shotgun();
        }
        throw new System.Exception("No weapon, but try get it");
    }

    private static MeleeWeapon getMeleeWeapon(int id)
    {
        switch (id)
        {
            case 1: return new Sword();
            case 2: return new Sword();
        }
        throw new System.Exception("No weapon, but try get it");
    }
}

public enum WeaponType
{
    RangeType,
    MeleeType
}


