using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;

public class StateUIMgr : MonoBehaviour {
    private static StateUIMgr _instance;
    public static StateUIMgr Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<StateUIMgr>();
            return _instance;
        }
    }

    public static UIType SlowDownState = new NodeType("StateItem/SlowDownItem");
    public static UIType DamageReductionState = new NodeType("StateItem/DamageReductionItem");
    public Transform TopPanel; // 拖拽

    public Dictionary<UIType, GameObject> StateTypes = new Dictionary<UIType, GameObject>();
    private void Start()
    {
        
    }

    public void AddBuff(IBuff buff)
    {
        switch (buff.getBuffType())
        {
            case BuffType.DamageReduction: AddState(DamageReductionState);break;
            case BuffType.SlowDown: AddState(SlowDownState);break;
        }
    }
    public void RemoveBuff(IBuff buff)
    {
        switch (buff.getBuffType())
        {
            case BuffType.DamageReduction: RemoveState(DamageReductionState); break;
            case BuffType.SlowDown: RemoveState(SlowDownState); break;
        }
    }

    protected void AddState(UIType type)
    {
        GameObject stateItem = Resources.Load<GameObject>(type.Path);
        stateItem = Instantiate(stateItem);
        StateTypes.Add(type, stateItem);
        stateItem.transform.SetParent(TopPanel);
        stateItem.GetComponent<BaseView>().OnEnter(type);
    }

    protected void RemoveState(UIType type)
    {
        GameObject item = StateTypes.GetValue(type);
        if (item != null)
        {
            item.GetComponent<BaseView>().OnExit(type);
            StateTypes.Remove(type);
            Destroy(item);
        }
        
    }
}


