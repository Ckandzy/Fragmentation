using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
	public class EquipInv : MonoBehaviour {
        private static EquipInv m_instance;
        public static EquipInv instance
        {
            get
            {
                if (m_instance == null) m_instance = FindObjectOfType<EquipInv>();
                return m_instance;
            }

        }
        public GameObject PrefabItem;
        private Transform WeaponSlot;
        private Transform OtherSlot;
        private Transform Der1Slot;
        private Transform Der2Slot;
        private Transform HeadSlot;
        private Transform ArmorSlot;

        private void Start()
        {
            WeaponSlot = transform.Find("WeaponSlot");
            OtherSlot = transform.Find("OtherSlot");
            Der1Slot = transform.Find("Der1Slot");
            Der2Slot = transform.Find("Der2Slot");
            HeadSlot = transform.Find("HeadSlot");
            ArmorSlot = transform.Find("ArmorSlot");
        }

        public void Add(int _itemid, GameObject _item)
        {
            WeaponInfo info = (WeaponInfo)AllObjectInfos.instance.GetObject(_itemid);
            PosOfBody pos = WeaponAL.GetPosOfBody(info.PosOfBody);
            if(WeaponAL.GetWeaponType(info.Type) == ObjectType.Weapon)
            {
                if(pos == PosOfBody.Armor && ArmorSlot.GetComponent<EquipSlot>().ItemID != -1)
                {
                    // 添加物体
                    ArmorSlot.GetComponent<EquipSlot>().Add(_itemid, _item);
                    return;
                }

                if (pos == PosOfBody.Der && Der1Slot.GetComponent<EquipSlot>().ItemID != -1)
                {
                    // 添加物体
                    Der1Slot.GetComponent<EquipSlot>().Add(_itemid, _item);
                    return;
                }

                if (pos == PosOfBody.Der && Der2Slot.GetComponent<EquipSlot>().ItemID != -1)
                {
                    // 添加物体
                    ArmorSlot.GetComponent<EquipSlot>().Add(_itemid, _item);
                    return;
                }

                if (pos == PosOfBody.Head && HeadSlot.GetComponent<EquipSlot>().ItemID != -1)
                {
                    // 添加物体
                    HeadSlot.GetComponent<EquipSlot>().Add(_itemid, _item);
                    return;
                }

                if (pos == PosOfBody.OneHandL && WeaponSlot.GetComponent<EquipSlot>().ItemID != -1)
                {
                    // 添加物体
                    WeaponSlot.GetComponent<EquipSlot>().Add(_itemid, _item);
                    return;
                }

                if (pos == PosOfBody.OneHandR && WeaponSlot.GetComponent<EquipSlot>().ItemID != -1)
                {
                    // 添加物体
                    WeaponSlot.GetComponent<EquipSlot>().Add(_itemid, _item);
                    return;
                }
            }
        }
    }
}


