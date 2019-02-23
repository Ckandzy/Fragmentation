using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MuyiFrame;
using Inv;
namespace UISystem
{
    public class Inventory : MonoBehaviour
    {
        private static Inventory m_instance;
        public static Inventory instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = FindObjectOfType<Inventory>();
                return m_instance;
            }
        }

        public Transform Item;
        public Transform Slot;
        public int SlotNum;

        public Transform DesPanel;
        public ObjectType nowType = ObjectType.Weapon;
        [HideInInspector]
        public Transform[] Slots;
        public Transform Container;

        private bool isFull = false;
        private Transform scrollPanel;

        // id itemdata
        public Dictionary<int, ItemData> itemDatasDic = new Dictionary<int, ItemData>();

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (DesPanel.gameObject.activeInHierarchy)
            {
                DesPanel.position = Input.mousePosition;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                int k = Random.Range(1001, 1003);
                Add(k);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                EmptyInventory();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                InventoryFlush(ObjectType.Weapon);
            }
        }

        
        protected void Init()
        {
            scrollPanel = transform.Find("Container/ScrollPanel");
            Container = transform.Find("Container");
            DesPanel = transform.Find("Container/DesPanel");
            DesPanel.gameObject.SetActive(false);
            Slots = new Transform[SlotNum];
            for (int i = 0; i < SlotNum; i++)
            {
                Slots[i] = Instantiate(Slot, scrollPanel);
                Slots[i].SetParent(scrollPanel);
                Slots[i].GetComponent<InvSlotBase>().SlotIndex = i;
            }

           //Add(1002);
        }

        public void RemoveItem(int _id, int reNum = 1)
        {
            foreach (Transform slot in Slots)
            {
                if(slot.GetComponent<InvSlotBase>().ItemID == _id)
                {
                    slot.GetComponent<InvSlotBase>().ItemNum -= 1;

                    ItemData itemData;
                    if (itemDatasDic.TryGetValue(_id, out itemData))
                    {
                        itemData.ItemNum -= reNum;
                        if(itemData.ItemNum == 0)
                        {
                            itemDatasDic.Remove(itemData.ItemID);
                        }
                    }
                }
            }
        }

        public void UpdateDic(int _id, int slotIndex)
        {
            ItemData item = null;
            if(itemDatasDic.TryGetValue(_id, out item))
            {
                item.InSlotsIndex = slotIndex;
            }
        }

        public void Add(int _id, int itemindex = -1, int addNum = 1)
        {
            if(itemindex != -1)
            {
                InventorySlot slot = Slots[itemindex].GetComponent<InventorySlot>();
                slot.Add(_id, Item.gameObject, addNum);
                return;
            }
            
            /*************************************************/
            // itemindex == -1 的情况，即没有给物品设置位置
            // 如果物品存在
            foreach (Transform slot in Slots)
            {
                if (slot.GetComponent<InventorySlot>().ItemID == _id)
                {
                    slot.GetComponent<InventorySlot>().ItemNum += addNum;
                    ItemData itemData;
                    if(itemDatasDic.TryGetValue(_id, out itemData))
                    {
                        itemData.ItemNum += addNum;
                    }

                    return;
                }
            }
            // 如果不存在
            int index = 0;
            foreach (Transform slot in Slots)
            {
                index++;
                if (slot.GetComponent<InventorySlot>().isEmpty)
                {
                    slot.GetComponent<InventorySlot>().Add(_id, Item.gameObject, addNum);
                    string spriteName = AllObjectInfos.instance.GetObject(_id).SpriteName;
                    Sprite sprite = Resources.Load<Sprite>(spriteName);
                    ItemData itemData = new ItemData(index - 1, _id, addNum, sprite, getType(AllObjectInfos.instance.GetObject(_id).Type));
                    if (!itemDatasDic.ContainsKey(_id)) itemDatasDic.Add(_id, itemData);
                    return;
                }
            }
        }
        /// <summary>
        /// 清空仓库
        /// </summary>
        public void EmptyInventory()
        {
            foreach(Transform slot in Slots)
            {
                slot.GetComponent<InventorySlot>().Empty();
            }
            Debug.Log("Empty Over");
        }

        public void InventoryFlush(ObjectType _type)
        {
            foreach(KeyValuePair<int, ItemData> items in itemDatasDic)
            {
                ItemData itemdata = items.Value;
                if(itemdata.objType == _type)
                {
                    InventorySlot slot = Slots[items.Value.InSlotsIndex].GetComponent<InventorySlot>();
                    slot.Add(itemdata.ItemID, Item.gameObject, itemdata.ItemNum);
                }
            }
            Debug.Log("Flush Over");
        }
        
        /// <summary>
        /// show object deriction
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_pos"></param>
        /// <param name="show"></param>
        public void ShowDes(int _id, Vector2 _pos, bool show = true)
        {
            if (show)
            {
                DesPanel.gameObject.SetActive(show);
                string name = AllObjectInfos.instance.GetObject(_id).Name;
                string des = AllObjectInfos.instance.GetObject(_id).Des;
                string effdes = AllObjectInfos.instance.GetObject(_id).EffectDes;
                DesPanel.GetComponentInChildren<Text>().text = name + "\n" + des + "\n" + effdes;
                DesPanel.position = _pos;
            }
            else
            {
                DesPanel.gameObject.SetActive(show);
            }
        }

        public void ChangeInventory(ObjectType type)
        {
            EmptyInventory();
            InventoryFlush(type);
            nowType = type;
        }
        
        /// <summary>
        /// use string to get the objecttype
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        private ObjectType getType(string _string)
        {
            switch (_string)
            {
                case "Food": return ObjectType.Food;
                case "Material": return ObjectType.Material;
                case "Weapon": return ObjectType.Weapon;

            }
            return ObjectType.Object;
        }
    }
}


