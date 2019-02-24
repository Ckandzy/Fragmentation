using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
public class GameTest : MonoBehaviour
{
    public static readonly PageType startpanel = new PageType("StartPanel");
    public static readonly PageType teskpanel = new PageType("TeskPanel");
    public static readonly PageType getteskpanel = new PageType("GetTeskPanel");
    public static readonly PageType TalkPanle = new PageType("DialogPanel");
    private void Start()
    {
        SingleTon<PageManager<UIManager>>.Instance.Push(startpanel);
        //SingleTon<PageManager<UIManager>>.Instance.Push(TalkPanle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SingleTon<PageManager<UIManager>>.Instance.Pop();
            return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SingleTon<PageManager<UIManager>>.Instance.Push(TalkPanle);
        }
    }
}

