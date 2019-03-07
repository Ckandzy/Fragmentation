using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Events;
using UnityEngine.EventSystems;
public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    public GameObject DesPanel;

    WeaponTaker weaponTaker;
    bool mouseEnter = false;

    private void Awake()
    {
        weaponTaker = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponTaker>();
    }
    private void Start()
    {
        CurrentSkill = weaponTaker.CurrentSkill;
        m_MaskTrans = CDMaskImage.GetComponent<RectTransform>();
        m_InitHight = m_MaskTrans.sizeDelta.y;
        DesPanel.SetActive(false);
    }

    public void Update()
    {
        ShowSkillLeftTime();
        if (mouseEnter)
        {
            FlushPotition();
        }
    }

    public void SkillInit(SkillBase _skill)
    {
        if(_skill != null)
        {
            SkillIconImage.sprite = _skill.SkillIcon;
            CDLeftTimeText.enabled = false;
            CurrentSkill = _skill;
        }
        else
        {
            SkillIconImage.sprite = null;
            CurrentSkill = null;
        }
        
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CurrentSkill != null)
        {
            DesPanel.SetActive(true);
            DesPanel.GetComponentInChildren<Text>().text = CurrentSkill.Des();
            mouseEnter = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEnter = false;
        DesPanel.SetActive(false);
    }
    private void FlushPotition()
    {
        DesPanel.transform.position = Input.mousePosition;
    }
}
