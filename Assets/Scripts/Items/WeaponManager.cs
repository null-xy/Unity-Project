using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    #region Singleton
    public static WeaponManager instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    // Use this for initialization
    public Transform weaponLeft;
    public Transform weaponRight;
    public Transform weaponBack;
    public Weapon[] currentWeapon;

    MeshRenderer[] currentMeshes;

    //Inventory inventory;
    public delegate void OnWeaponChanged(Weapon newItem, Weapon oldItem);
    public OnWeaponChanged onWeaponChanged;
    
    void Start()
    {
        //inventory = Inventory.instance;
       // int numSlots = System.Enum.GetNames(typeof(WeaponSlot)).Length;
        int numSlots = System.Enum.GetNames(typeof(WeaponType)).Length;
        currentWeapon = new Weapon[numSlots];
        currentMeshes = new MeshRenderer[numSlots];
        //playeraAnimation.PullBow += OnPullBow;
        //EquipDefaultItems();
        //inventory.OnRightClickEvent += inventoryRightClick;
    }

    public void Equip(Weapon newItem)
    {
        //原始武器分类弓，剑，匕首，箭    int slotIndex = (int)newItem.weaponSlot;
        //新武器分类，左手，右手，双手
        int slotIndex = (int)newItem.weaponType;
        // Unequip(slotIndex);
        Weapon oldItem = Unequip(slotIndex);
        //------add old item to inventory
        if (currentWeapon[slotIndex] != null)
        {
            oldItem = currentWeapon[slotIndex];
            //inventory.Add(oldItem);
            //换下旧武器
        }
        if (onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(newItem, oldItem);
        }
        currentWeapon[slotIndex] = newItem;
        //GameObject go = Instantiate(newItem, Vector3.zero, Quaternion.identity) as GameObject;
        MeshRenderer newMesh = Instantiate<MeshRenderer>(newItem.mesh);
        newMesh.name = newItem.name;
        if (newItem.weaponSlot == WeaponSlot.Arrow)
        {
            newMesh.transform.parent = weaponRight;
            newMesh.transform.localPosition = Vector3.zero;
            newMesh.transform.localRotation = Quaternion.identity;
            newMesh.transform.localScale = new Vector3(1, 1, 1);
        }
        if (newItem.weaponSlot == WeaponSlot.Bow)
        {
            newMesh.transform.parent = weaponBack;
            newMesh.transform.localPosition = Vector3.zero;
            newMesh.transform.localRotation = Quaternion.identity;
            newMesh.transform.localScale = new Vector3(2, 2, 2);
        }

        currentMeshes[slotIndex] = newMesh;
    }
    public Weapon Unequip(int slotIndex)
    {
        if (currentWeapon[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            Weapon oldItem = currentWeapon[slotIndex];
            //inventory.Add(oldItem);
            currentWeapon[slotIndex] = null;
            if (onWeaponChanged != null)
            {
                onWeaponChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }
    public void UnequipAll()
    {
        for (int i = 0; i < currentWeapon.Length; i++)
        {
            Unequip(i);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }
}