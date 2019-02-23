using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PosOfBody
{
    /// <summary>
    /// 单手，副
    /// </summary>
    OneHandL,
    /// <summary>
    /// 单手，主
    /// </summary>
    OneHandR,
    /// <summary>
    /// 双手
    /// </summary>
    TwoHand,
    /// <summary>
    /// 头盔
    /// </summary>
    Head,
    /// <summary>
    /// 防具
    /// </summary>
    Armor,
    /// <summary>
    /// 装饰
    /// </summary>
    Der, 
    /// <summary>
    /// 其他
    /// </summary>
    Other

}

public enum ObjectType
{
    Object,
    Weapon,
    Food,
    Material
}
