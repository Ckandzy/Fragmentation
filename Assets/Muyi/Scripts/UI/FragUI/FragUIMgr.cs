using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
public class FragUIMgr : UIManager{
    protected override void Init()
    {
        canves = GameObject.Find("UICanvas").GetComponent<Transform>();
    }
}


