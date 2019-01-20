using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "inventory/Weapon")]
public class Weapon : Item {

    // Use this for initialization
    public WeaponSlot weaponSlot;
    public WeaponType weaponType;
    public MeshRenderer mesh;
    public int armorModifier;
    public int damageModifer;
    public int attackRangeModifer;
    public int attackSpeedModifer;
    public override void Use()
    {
        base.Use();
        WeaponManager.instance.Equip(this);
        //equip the item
        //RemoveFromInventory();
        //remove it form the inventory
    }
    public override void UnEquip()
    {
        WeaponManager.instance.Unequip((int)this.weaponType);
    }

}
public enum WeaponSlot { Bow, Sword, Dagger, Shield, Arrow, Quiver }
public enum WeaponType { LeftHand, RightHand, TwoHanded}
