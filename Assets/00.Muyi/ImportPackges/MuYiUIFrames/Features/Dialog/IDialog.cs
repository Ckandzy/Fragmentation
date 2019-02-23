using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDialog : MonoBehaviour {
    public Dialog DialogAsset;
    /// <summary>
    /// on Enter Dialog
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// Show next words
    /// </summary>
    public abstract void Next();
    /// <summary>
    /// Previous 上一个
    /// </summary>
    public abstract void Previous();
    /// <summary>
    /// When talk over
    /// </summary>
    public abstract void Over();
    /// <summary>
    /// if in this dialog not exit
    /// </summary>
    public abstract void UpdateInDialog();
}

