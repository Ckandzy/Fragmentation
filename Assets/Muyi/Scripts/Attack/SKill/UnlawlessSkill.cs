using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 无法无天（冷却时间30秒）
/// 无序+暴乱碎片激活
/// 获得霸体，身边生成力场对靠近的敌人持续造成伤害（X点每秒），同时回复生命（5点每秒），持
/// 续6秒
/// </summary>
public class UnlawlessSkill : SkillBase
{
    public GameObject SkillPrefab;
    public string SkillObjPath;
    public UnlawlessSkill() { Init(); }

    public void Init()
    {
        ReadyUse = true;
        buffs = new List<IBuff>();
        CD = 30;
        SkillObjPath = "";
    }

    public override void SkillUpdate()
    {
        if(m_WaitTime < CD)
             m_WaitTime += Time.deltaTime;
        else
        {
            ReadyUse = true;
        }
    }

    public override void OnSkillOver() { }

    public override void UseSkill(Transform _trans)
    {
        SkillPrefab = Resources.Load<GameObject>(SkillObjPath);
        ReadyUse = false;
        GameObject skillObj = MonoBehaviour.Instantiate(SkillPrefab, _trans);
        m_WaitTime = 0;
    }
}
