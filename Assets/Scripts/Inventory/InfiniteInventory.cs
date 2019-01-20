using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfiniteInventory : Inventory {

    [SerializeField]    GameObject itemSlotPrefab;
    [SerializeField]    int maxSlots;

    //public event Action<InventorySlot> OnPointerEnterEvent;
    //public event Action<InventorySlot> OnPointerExitEvent;

    [SerializeField]
    public List<InventorySlot> allSlots = new List<InventorySlot>();
        
    public int MaxSlots
    {
        get { return maxSlots; }
        set { SetMaxSlots(value); }
    }
    protected override void OnValidate()
    {
        if (itemParent != null)// && partItemParent!=null
        {   itemParent.GetComponentsInChildren<InventorySlot>(includeInactive: true, result: slots);
            //partItemParent.GetComponentsInChildren<InventorySlot>(includeInactive: true, result: sortedSlots);
        }
        if (!Application.isPlaying)
        {
            SetStartingItems();
        }
    }
    protected override void Start()
    {
        SetMaxSlots(maxSlots);
        base.Start();

    }
    public override bool CanAddItem(Item item, int Amount = 1)
    {
        return true;
    }
    public override bool Add(Item item)
    {
        while (!base.CanAddItem(item))
        {
            MaxSlots += 1;
            base.Start();
        }
        return base.Add(item);
    }
    private void SetMaxSlots(int value)
    {
        if (value < 0)
        {
            maxSlots = 1;
        }
        else
        {
            maxSlots = value;
        }

        if (maxSlots < slots.Count)
        {
            for(int i = maxSlots; i < slots.Count; i++)
            {
                Destroy(slots[i].transform.parent.gameObject);
            }
            int diff = slots.Count - maxSlots;
            slots.RemoveRange(maxSlots, diff);
        }else if (maxSlots > slots.Count)
        {
            int diff = maxSlots - slots.Count;
            for (int i = 0; i < diff; i++)
            {
                GameObject slotObj = Instantiate(itemSlotPrefab);
                slotObj.transform.SetParent(itemParent, worldPositionStays: false);
                slots.Add(slotObj.GetComponentInChildren<InventorySlot>());
            }
        }
    }

}
