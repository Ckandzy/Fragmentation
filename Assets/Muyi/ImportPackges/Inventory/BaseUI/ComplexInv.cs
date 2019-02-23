using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;
using UISystem;

// 更改名为ComplexInv
namespace Inv
{
	public class ComplexInv : BaseComplexInv {
        #region normal inv

        #endregion


        #region extend BaseComplexInv
        private Transform selectFunBar;
        public BarNode NowBarNode;

        private void Start()
        {
            selectFunBar = GameObject.Find("SelectFunBar").transform;
            AddNode(Foods_Btn);
            AddNode(Material_Btn);
            AddNode(Weapons_Btn);
            AddNode(SkillCard_Btn);

            GetBar(selectFunBar, BarNodes);
        }

        public void SelectedIn(BarNode node)
        {
            NowBarNode = node;
        }

        public override void Show(BarNode node)
        {
            Debug.Log(node.NodeType);
            Inventory.instance.ChangeInventory(node.NodeType);
        }

        private readonly BarNode Foods_Btn = new BarNode("Foods_Btn", ObjectType.Food);
        private readonly BarNode Material_Btn = new BarNode("Material_Btn", ObjectType.Material);
        private readonly BarNode Weapons_Btn = new BarNode("Weapons_Btn", ObjectType.Weapon);
        private readonly BarNode SkillCard_Btn = new BarNode("SkillCard_Btn");
        #endregion
    }
}


