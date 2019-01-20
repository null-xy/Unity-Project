using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour,IInventoryItem {
    #region Singleton
    public delegate void OnItemChange();
    public OnItemChange onItemChangedCallBack;
    public static Inventory instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one instance of inventory");
            return;
        }
        instance = this;
        
    }
    #endregion
    public event Action<InventorySlot> OnRightClickEvent;
    public event Action<InventorySlot> OnPointerEnterEvent;
    public event Action<InventorySlot> OnPointerExitEvent;

    [SerializeField] public List<InventorySlot> slots;
    //------------------------------------===========================----------------------
    //[SerializeField] public List<InventorySlot> sortedSlots = new List<InventorySlot>();
    //public Transform partItemParent;
    //[SerializeField] public GameObject itemSlotPrefab;
    //----------------------------------------------------
    [SerializeField] Item[] startingItems;
    public Transform itemParent;

    protected virtual void OnValidate()
    {
        if (itemParent != null)//&& partItemParent != null
        {
            itemParent.GetComponentsInChildren<InventorySlot>(includeInactive: true, result: slots);
           // partItemParent.GetComponentsInChildren<InventorySlot>(includeInactive: true, result: sortedSlots);
        }
    }

    protected virtual void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].OnRightClickEvent += slot => { if (OnRightClickEvent != null) OnRightClickEvent(slot); };
            slots[i].OnPointerEnterEvent += slot => { if (OnPointerEnterEvent != null) OnPointerEnterEvent(slot); };
            slots[i].OnPointerExitEvent += slot => { if (OnPointerExitEvent != null) OnPointerExitEvent(slot); };
        }
    }

    protected void SetStartingItems()
    {
        int i = 0;
        for (; i < startingItems.Length && i < slots.Count; i++)
        {
            slots[i].item = startingItems[i].GetCopy();
            slots[i].amount = 1;
        }
        for (; i < slots.Count; i++)
        {
            slots[i].item = null;
            slots[i].amount = 0;
        }
    }
    //物品分页
    /*public void AddItemTopartGrid(List<InventorySlot> items)
    {
        for(int i = 0; i < items.Count; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab);
            slotObj.transform.SetParent(itemParent, worldPositionStays: false);
            slots.Add(slotObj.GetComponentInChildren<InventorySlot>());
        }
    }
    void UpdateSortedItem()
    {
        SortItemByEquipment();
    }
    public void SortAllItems()
    {
        ClearSortedSlots();
        foreach (InventorySlot itemSlot in slots)
        {
            sortedSlots.Add(itemSlot);
        }
        ShowSortedItem();
    }
    public void SortItemByEquipment()
    {
        ClearSortedSlots();
        foreach (InventorySlot itemSlot in slots)
        {
            if (itemSlot.item is Equipment)
            {
                sortedSlots.Add(itemSlot);
            }
        }
        ShowSortedItem();
    }
    //----------------
    private void ShowSortedItem()
    {
        for (int i = 0; i < sortedSlots.Count; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab);
            slotObj.transform.SetParent(partItemParent, worldPositionStays: false);
            InventorySlot slotInfo = slotObj.GetComponent<InventorySlot>();
            slotInfo.item = sortedSlots[i].item;
            slotInfo.amount = sortedSlots[i].amount;
            slotInfo.OnRightClickEvent += slot => { if (OnRightClickEvent != null) OnRightClickEvent(slot); };
            slotInfo.OnPointerEnterEvent += slot => { if (OnPointerEnterEvent != null) OnPointerEnterEvent(slot); };
            slotInfo.OnPointerExitEvent += slot => { if (OnPointerExitEvent != null) OnPointerExitEvent(slot); };
        }
    }

    private void ClearSortedSlots()
    {
        foreach (Transform t in partItemParent)
        {
            Destroy(t.gameObject);
        }
        sortedSlots.Clear();
    }
    */
    public void AutoOrganize()
    {
        int k = 0;
        int i = 0;
        for (i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                k = i;
                break;
            }
        }for(int j = i; j < slots.Count; j++)
        {
            if (slots[j].item != null)
            {
                Item item = slots[j].item;
                slots[k].item = item;
                slots[k].amount = slots[j].amount;
                slots[j].item = null;
                slots[j].amount = 0;
                break;
            }
        }
    }

    public virtual bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == null || slots[i].item.ID == item.ID&& slots[i].amount<item.MaximumStack)
                {
                    slots[i].item = item;
                    slots[i].amount++;
                    return true;
                }
            }return false;
            
            /*for (int i = 0; i < items.Count; i++)
            {
                if (slots[i].item==null || items[i].name == item.name)
                {
                    slots[i].item = item;
                    slots[i].amount++;
                    if (onItemChangedCallBack != null)
                        onItemChangedCallBack.Invoke();
                    return true;
                }
            }*/
            //items.Add(item);
            //if(onItemChangedCallBack!=null)
            //onItemChangedCallBack.Invoke();
        }return false;
    }
    public bool Remove(Item item)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                if (slots[i].itemEquipImage.enabled && slots[i].amount==1)
                {
                    slots[i].UseItem();//unequip equipment||weapon before drop it
                }
                slots[i].amount--;
                if (slots[i].amount == 0) {
                    slots[i].item = null;
                }
                //========
                if (onItemChangedCallBack != null)
                    onItemChangedCallBack.Invoke();
                    return true;
            }
        }
        return false;
    }
    //removeitem（itemID）
    public Item RemoveItem(string itemID)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            Item item = slots[i].item;
            if (item != null && item.ID == itemID)
            {
                if (slots[i].itemEquipImage.enabled && slots[i].amount == 1)
                {
                    slots[i].UseItem();
                }
                slots[i].amount--;
                if (slots[i].amount == 0) {
                    slots[i].item = null;
                }
                return item;
            }
        }
        return null;
    }

    /*public bool ContainsItem(string itemID)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item.ID == itemID)
            {
                return true;
            }
        }return false;
    }*/
    public Weapon searchArrow()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].item is Weapon)
            {
                Weapon arrow = slots[i].item as Weapon;
                if (arrow.weaponSlot == WeaponSlot.Arrow)
                {
                    return arrow;
                }
            }
        }
        return null;
    }
    public int searchItemSlot(string itemID)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Item item = slots[i].item;
            if (item != null && item.ID == itemID)
            {
                return i;
            }
        }return -1;
    }
    public int ItemCount(string itemID)
    {
        int number = 0;
        for(int i = 0; i < slots.Count; i++)
        {
            Item item = slots[i].item;
            if (item !=null && item.ID == itemID)
            {
                number+=slots[i].amount;
            }
        }return number;
    }

    public virtual bool CanAddItem(Item item,int Amount=1)
    {
        int freeSpace = 0;
        foreach(InventorySlot slot in slots)
        {
            if (slot.item == null || slot.item.ID == item.ID)
            {
                freeSpace += item.MaximumStack - slot.amount;
            }
        }
        return freeSpace >= Amount;
    }

    public void Clear()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].item = null;
            slots[i].amount = 0;
        }
    }
}
