﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    private static SkillSlot _instance;
    public static SkillSlot Instance
    {
        get { if (_instance == null) _instance = FindObjectOfType<SkillSlot>(); return _instance; }
    }

    public Image SkillIconImage;
    public Image CDMaskImage;
    public Text CDLeftTimeText;
    public SkillBase CurrentSkill;

    WeaponTaker weaponTaker;

    private void Awake()
    {
        weaponTaker = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponTaker>();
    }
    private void Start()
    {
        CurrentSkill = weaponTaker.CurrentSkill;
        m_MaskTrans = CDMaskImage.GetComponent<RectTransform>();
        m_InitHight = m_MaskTrans.sizeDelta.y;
    }

    public void Update()
    {
        ShowSkillLeftTime();
    }

    public void SkillInit(SkillBase _skill)
    {
        CurrentSkill = _skill;
        SkillIconImage.sprite = _skill.SkillIcon;
        CDLeftTimeText.enabled = false;
    }

    public void SkillOver()
    {

    }

    RectTransform m_MaskTrans;
    float m_InitHight;
    float m_CurrentHeight;

    public void ShowSkillLeftTime()
    {
        if (CurrentSkill == null)
        {
            SkillIconImage.sprite = null;
            CDLeftTimeText.enabled = false;
            return;
        }
        if (CurrentSkill.MSkillStatus == SkillStatusEnum.Ready)
        {
            SkillIconImage.sprite = CurrentSkill.SkillIcon;
            m_MaskTrans.sizeDelta = new Vector2(m_MaskTrans.sizeDelta.x, 0);
            CDLeftTimeText.enabled = false;
        }
        else if(CurrentSkill.MSkillStatus == SkillStatusEnum.InCD)
        {
            CDLeftTimeText.enabled = true;
            m_CurrentHeight = m_InitHight * CurrentSkill.LeftTime / CurrentSkill.CD;
            m_MaskTrans.sizeDelta = new Vector2(m_MaskTrans.sizeDelta.x, m_CurrentHeight);
            CDLeftTimeText.text = ((int)CurrentSkill.LeftTime).ToString();
        }
        else if (CurrentSkill.MSkillStatus == SkillStatusEnum.Continued)
        {
            CDLeftTimeText.enabled = true;
            CDLeftTimeText.text = "持续中";
        }
    }
}
