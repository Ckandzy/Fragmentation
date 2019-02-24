using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiFrame
{
	public class UIManager{
        public UIManager() { Init(); }
        protected Transform canves;
        Dictionary<UIType, GameObject> PagesDic =
            new Dictionary<UIType, GameObject>();

        protected virtual void Init()
        {
            canves = GameObject.Find("Canvas").GetComponent<Transform>();
        }

        public GameObject GetByUIType(UIType _type)
        {
            return PagesDic.GetValue(_type);
        }

        public bool Add(UIType _type, GameObject _game)
        {
            return PagesDic.AddKeyValues(_type, _game);
        }

        public bool Exist(UIType _type)
        {
            return PagesDic.ContainsKey(_type);
        }

        public GameObject GetPageGameObj(UIType _type)
        {
            if (Exist(_type))
            {
                return GetByUIType(_type);
            }
            else
            {
                return Instantiate(_type);
            }
        }
        // 预设体的中心要放好 -- 可以通过动画调节位置
        protected GameObject Instantiate(UIType _type)
        {
            //Debug.Log(_type.Path);
            Transform trans = canves.Find(_type.name);
            GameObject gameobj;
            if (trans != null)
            {
                gameobj = trans.gameObject;
                PagesDic.Add(_type, gameobj);
                return trans.gameObject;
            }
            //Debug.Log(gameobj);
            gameobj = Resources.Load<GameObject>(_type.Path) as GameObject;
            GameObject obj = GameObject.Instantiate(gameobj);
            obj.transform.SetParent(canves);
            obj.transform.localPosition = Vector2.zero;
            obj.name = gameobj.name; // 可以不加
            // 生成时这一句必须加 -- 防止跑偏
            obj.GetComponent<RectTransform>().sizeDelta = gameobj.GetComponent<RectTransform>().sizeDelta;
            PagesDic.Add(_type, obj);
            return obj;
        }

        public bool Delete(UIType _type)
        {
            GameObject.Destroy(GetByUIType(_type));
            return PagesDic.Remove(_type);
        }
	}

    public static class DicExtensions
    {

        public static GameObject GetValue<T>(this Dictionary<T, GameObject> dic, T _type)
        {
            GameObject game;
            if (dic.TryGetValue(_type, out game))
            {
                return game;
            }
            return null;
        }

        public static bool AddKeyValues<T>(this Dictionary<T, GameObject> dic, T _type, GameObject game)
        {
            if (dic.ContainsKey(_type))
            {
                return false;
            }
            dic.Add(_type, game);
            return true;
        }
    }
}


