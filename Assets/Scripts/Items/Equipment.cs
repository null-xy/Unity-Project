using UnityEngine;
[CreateAssetMenu(fileName ="New Equipment",menuName = "inventory/Equipment")]
public class Equipment : Item {
    public EquipmentSlot equipSlot;
    //public WeaponSlot weaponSlot;
    public SkinnedMeshRenderer mesh;
    //public MeshRenderer meshNoraml;
    public int armorModifier;
    public int damageModifer;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        //equip the item
        //RemoveFromInventory();
        //remove it form the inventory
    }
    public override void UnEquip()
    {
        EquipmentManager.instance.Unequip((int)this.equipSlot);
    }
}
public enum EquipmentSlot { Head, Face, Chest, Hands, Feet, LeftWeapon, RightWeapon}
//public enum WeaponSlot {Bow, Sword, Dagger, Shield, Arrow}