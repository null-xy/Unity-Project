using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {
    #region Singleton
    public static EquipmentManager instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    public Equipment[] defaultItems;
    public SkinnedMeshRenderer targetMesh;
    //weapon mesh

    public GameObject weaponLeft;
    Equipment[] currentEquipment;

    SkinnedMeshRenderer[] currentMeshes;
    //weapon mesh
    //MeshRenderer[] currentMeshesWeapon;

    //Inventory inventory;
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    void Start()
    {
        //inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];
        EquipDefaultItems();
    }
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
       // Unequip(slotIndex);
        Equipment oldItem = Unequip(slotIndex);
        //------add old item to inventory-------------
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            //inventory.Add(oldItem);
        }
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        currentEquipment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        if(newItem.equipSlot == EquipmentSlot.LeftWeapon)
        {
            newMesh.transform.parent = weaponLeft.transform;
            newMesh.transform.localPosition = Vector3.zero;
            newMesh.transform.localRotation = Quaternion.identity;
            newMesh.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            newMesh.transform.parent = targetMesh.transform;
            newMesh.bones = targetMesh.bones;
            newMesh.rootBone = targetMesh.rootBone;
        }
        
        currentMeshes[slotIndex] = newMesh;
    }
    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            Equipment oldItem = currentEquipment[slotIndex];
            //inventory.Add(oldItem);
            currentEquipment[slotIndex] = null;            
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);                
            }
            return oldItem;
        }
        return null;
    }
    /*public void UnequipAll()
    {
        for(int i =0; i<currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipDefaultItems();
    }*/
    void EquipDefaultItems()
    {
        foreach(Equipment item in defaultItems)
        {
            Equip(item);
        }
    }

    void Update()
    { 
        for(int i = 0; i < defaultItems.Length; i++)
        {
            int slotIndex = (int)defaultItems[i].equipSlot;
            if (currentEquipment[slotIndex] == null)
            {
                Equip(defaultItems[i]);
            }
        }
        /*if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }*/
    }
}
