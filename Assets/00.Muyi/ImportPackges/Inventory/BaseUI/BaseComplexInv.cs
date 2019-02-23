using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using UnityEngine.UI;
namespace Inv
{
	public abstract class BaseComplexInv : MonoBehaviour {
        protected Dictionary<BarNode, GameObject> BarNodesDic = new Dictionary<BarNode, GameObject>();
        protected List<BarNode> BarNodes;  // 切换仓库的按钮，如example中的技能卡-武器-等按钮
       
        public virtual void AddNode(BarNode _node)
        {
            if(BarNodes == null)
            {
                BarNodes = new List<BarNode>();
            }
            if (BarNodes.Contains(_node)) return;
            BarNodes.Add(_node);
        }

        public virtual void AddNode(List<BarNode> barNodes)
        {
            BarNodes = barNodes;
        }

        public virtual void GetBar(Transform _father, BarNode node)
        {
            if (BarNodes.Contains(node)) return;
            Transform trans = GameObject.Instantiate(Resources.Load(node.Path) as GameObject).transform;
            trans.SetParent(_father);
            trans.GetComponent<Button>().onClick.AddListener(() => { Show(node); });
            BarNodesDic.Add(node, trans.gameObject);
        }

        public virtual void GetBar(Transform _father, List<BarNode> nodes)
        {
            foreach(BarNode node in nodes)
            {
                Transform trans = GameObject.Instantiate(Resources.Load(node.Path) as GameObject).transform;
                trans.SetParent(_father);
                trans.GetComponent<Button>().onClick.AddListener(() => { Show(node); });
                BarNodesDic.Add(node, trans.gameObject);
            }
        }

        public abstract void Show(BarNode node);
    }
}


