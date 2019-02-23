using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class AllObjectInfos : MonoBehaviour
{
    //private AllObjectInfos() { Init(); }
    private static AllObjectInfos m_instance;
    public static AllObjectInfos instance
    {
        get
        {
            m_instance = FindObjectOfType<AllObjectInfos>();
            return m_instance;
        }
    }
    [SerializeField]
    private TextAsset[] ObjectAsset;
    private List<WeaponInfo> m_Datas;

    private Dictionary<int, ObjectInfo> objectDic = new Dictionary<int, ObjectInfo>();

    private void Awake()
    {
        m_instance = this;
        Init();
    }

    private void Init()
    {
        //foreach(TextAsset texts in ObjectAsset){
        //    m_Datas = JsonToObject.JsonToObject_ByJsonContent<WeaponInfo>(texts.text);
        //}
        //Debug.Log(m_Datas[0].ID + " " + m_Datas[0].Name + " " + m_Datas[0].SpriteName);
        //foreach(WeaponInfo info in m_Datas)
        //{
        //    objectDic.Add(info.ID, info);
        //}
        
    }
    /// <summary>
    /// 通过ID获得物体
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public ObjectInfo GetObject(int _id)
    {
        ObjectInfo info;
        if (objectDic.TryGetValue(_id, out info))
        {
            return info;
        }
        return null;
    }
}



