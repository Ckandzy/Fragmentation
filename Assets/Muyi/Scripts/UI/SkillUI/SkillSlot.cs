using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image SkillIconImage;
    public Image CDMaskImage;
    public Text CDLeftTimeText;
    public SkillBase CurrentSkill;

    private void Start()
    {
        CurrentSkill = new UnlawlessSkill();
    }

    public void Update()
    {
        m_MaskTrans = CDMaskImage.GetComponent<RectTransform>();
        m_InitHight = m_MaskTrans.sizeDelta.y;
    }

    RectTransform m_MaskTrans;
    float m_InitHight;
    float m_CurrentHeight;
    public void ShowSkillLeftTime()
    {
        if (CurrentSkill.ReadyUse)
        {
            m_MaskTrans.sizeDelta = new Vector2(m_MaskTrans.sizeDelta.x, 0);
        }
        else
        {
            m_CurrentHeight = m_InitHight * CurrentSkill.LeftTime / CurrentSkill.CD;
            m_MaskTrans.sizeDelta = new Vector2(m_MaskTrans.sizeDelta.x, m_CurrentHeight); 
        }
    }
}
