using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAL : MonoBehaviour
{
    public static ObjectType GetWeaponType(string _typeString)
    {
        switch (_typeString)
        {
            case "Weapon": return ObjectType.Weapon;
            case "Object":return ObjectType.Object;
            case "Material":return ObjectType.Material;
            case "Food":return ObjectType.Food;
        }

        return ObjectType.Object;
    }

    public static PosOfBody GetPosOfBody(string _posString)
    {
        switch (_posString)
        {
            case "OneHandL": return PosOfBody.OneHandL;
            case "OneHandR": return PosOfBody.OneHandR;
            case "TwoHand": return PosOfBody.TwoHand;
            case "Head": return PosOfBody.Head;
            case "Armor": return PosOfBody.Armor;
            case "Der": return PosOfBody.Der;
        }
        return PosOfBody.Other;
    }

}

