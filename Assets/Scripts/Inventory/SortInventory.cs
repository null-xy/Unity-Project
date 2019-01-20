using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SortInventory : Inventory
{
    [SerializeField]
    Button allSortButton;
    [SerializeField]
    Button equipmentSortButton;
    void start()
    {
        this.onItemChangedCallBack += UpdateSortedItem;
    }
    void UpdateSortedItem()
    {
       // SortItemByEquipment();
    }
}
