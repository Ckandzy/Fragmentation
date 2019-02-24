using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiFrame;

namespace MuyiFrame
{
	public class BarNode : UIType {
        public ObjectType NodeType = ObjectType.Weapon;
        public BarNode(string _path, ObjectType type ): base(_path)
        {
            NodeType = type;
        }

        public BarNode(string _path) : base(_path)
        {
        }
    }
}


