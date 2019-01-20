using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharactorStats {

	// Use this for initialization
	void Start () {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        WeaponManager.instance.onWeaponChanged += OnWeaonChanged;

    }
	void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null) {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifer);
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifer);
        }
        
    }
    void OnWeaonChanged(Weapon newItem, Weapon oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifer);
            attackRange.AddModifier(newItem.attackRangeModifer);
            attackSpeed.AddModifier(newItem.attackSpeedModifer);
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifer);
            attackRange.RemoveModifier(oldItem.attackRangeModifer);
            attackSpeed.RemoveModifier(oldItem.attackSpeedModifer);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
