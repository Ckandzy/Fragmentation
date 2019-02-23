using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inv;

namespace MuyiFrame
{

    // 处理多个并行UI， 且随时可以跳转，但只能存在一个，如同仓库的分类，武器，材料等并行,但
    // 使用事件通知机制
    // 当跳转到一个UI时， 由Mgr通知其余所有的UI这个事件， 做出处理
    public class NotifyPageManager {
        protected NotifyPageManager() { }
        public Dictionary<UIType, GameObject> NotifiedUIDic = new Dictionary<UIType, GameObject>();

        public virtual void NotifyExcept(UIType _excepttype)
        {
            //if (NotifiedUIDic == null) NotifiedUIDic = new Dictionary<UIType, GameObject>();
            Debug.Log(NotifiedUIDic.Count);
            foreach (KeyValuePair<UIType, GameObject> pair in NotifiedUIDic)
            {
                if (_excepttype == pair.Key) continue;
                Debug.Log(pair.Value);
                pair.Value.GetComponent<NotifiedAnima>().Notify(pair.Key);
            }
        }

        public virtual void Notify(UIType _type)
        {
            GameObject obj;
            if(NotifiedUIDic.TryGetValue(_type, out obj))
            {
                obj.GetComponent<NotifiedAnima>().Notify(_type);
            }
        }

        public virtual void ChangeToUI(UIType _type)
        {
            //if (!NotifiedUIDic.ContainsKey(_type))
            //{
            //    SingleTon<UIManager>.Instance.GetPageGameObj(_type);
            //}
            GameObject obj;
            if(NotifiedUIDic.TryGetValue(_type, out obj))
            {
                Debug.Log(obj);
                obj.GetComponent<BaseView>().OnEnter(_type);
                NotifyExcept(_type);
            }
        }
    }
}
