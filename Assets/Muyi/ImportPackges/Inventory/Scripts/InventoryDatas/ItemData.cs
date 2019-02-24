using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
	public class ItemData{
        public int InSlotsIndex;
        public int ItemID;
        public int ItemNum;
        public Sprite ItemSprite;
        public ObjectType objType;
        public ItemData(int index = 0, int itemID = 0, int itemNum = 0, Sprite itemSprite = null, ObjectType _type = ObjectType.Object)
        {
            this.InSlotsIndex = index;
            this.ItemID = itemID;
            this.ItemNum = itemNum;
            this.ItemSprite = itemSprite;
            this.objType = _type;
        }
	}
}


