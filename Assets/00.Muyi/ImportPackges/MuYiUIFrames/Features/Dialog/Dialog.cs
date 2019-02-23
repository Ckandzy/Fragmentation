using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 简单对话，一条一条的向下
/// </summary>
[CreateAssetMenu(menuName = "MuyiFrame/NormalDialog", fileName = "NormalDialog")]
public class Dialog : ScriptableObject {
    public Sprite[] TalkerSprite;
    public string[] Words;
}

/// <summary>
/// 复杂对话，有可能出现分支，例如根据玩家的正义值，获得不同的对话
/// 未完成--
/// </summary>
[CreateAssetMenu(menuName = "MuyiFrame/ComplexDialog", fileName = "ComplexDialog")]
public class ComplexDialog : ScriptableObject
{
    public Sprite[] TalkerSprite;
    public string[] Words;
}


