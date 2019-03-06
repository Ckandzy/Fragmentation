using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class IData : Data
{
    public IData(CharacterType _StatusType, float _maxHp, float _hp, 
        List<IBuff> _m_StatusBuffs, bool _isStoic, StatisticsFrag _frags)
    {
        StatusType = _StatusType;
        maxHp = _maxHp;
        hp = _hp;
        m_StatusBuffs = _m_StatusBuffs;
        isStoic = _isStoic;
        frags = _frags;
    }

    public CharacterType StatusType;
    public float maxHp = 100;
    public float hp = 100;
    public List<IBuff> m_StatusBuffs;
    public bool isStoic = false;
    public StatisticsFrag frags;
}
