using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using Gamekit2D;

public class FragUI : MonoBehaviour {
    public UIType FragmentPanel = new NodeType("FragmentPanel");

    private void Start()
    {
        // 实例化
        //SingleTon<PageManager<FragUIMgr>>.Instance.Push(FragmentPanel);
        //SingleTon<PageManager<FragUIMgr>>.Instance.Pop();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        
    }

    public void Update()
    {
        if (PlayerInput.Instance.Tab.Down)
        { 
            SingleTon<PageManager<FragUIMgr>>.Instance.Push(FragmentPanel);
        }
        if (PlayerInput.Instance.Exit.Down)
        {
            SingleTon<PageManager<FragUIMgr>>.Instance.Pop();
        }
    }
}


