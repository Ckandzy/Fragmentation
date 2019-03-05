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
    public UnlawlessSkill() { }

    float durationTimer = 0;
    public override void SkillEnter()
    {
        MSkillStatus = SkillStatusEnum.Ready;
        buffs = new List<IBuff>();
        CD = 30;
        DurationTime = 5;
        SkillIcon = Resources.Load<Sprite>("SkillIcon/UnlawlessSkill");
    }

    public override void SkillUpdate()
    {
        if(MSkillStatus == SkillStatusEnum.Ready)
        {
            m_WaitTime = 0;
            durationTimer = 0;
        }
        else if (MSkillStatus == SkillStatusEnum.InCD)
        {
            m_WaitTime += Time.deltaTime;
            if (m_WaitTime >= CD)
                MSkillStatus = SkillStatusEnum.Ready;
        }
        else if (MSkillStatus == SkillStatusEnum.Continued)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer >= DurationTime)
            {
                MSkillStatus = SkillStatusEnum.InCD;
                OnSkillOver();
            }
        }
    }

    public override void OnSkillOver()
    {
        
    }

    public override void UseSkill(Transform _trans)
    {
        if (MSkillStatus == SkillStatusEnum.Ready)
        {
            //SkillPrefab = Resources.Load<GameObject>(SkillObjPath);
            VFXControllerM.Instance.MakeForceField(_trans, DurationTime);
            MSkillStatus = SkillStatusEnum.Continued;
            m_WaitTime = 0;
            _trans.parent.GetComponent<Status>().AddStatusBuff(buffs);
            _trans.parent.GetComponent<Status>().RegisteredTakeDamger(VFXControllerM.Instance.ForceField.GetComponent<TakeDamager>());
        }
    }
}
