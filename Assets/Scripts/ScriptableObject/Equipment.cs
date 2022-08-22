using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public EquipmentSlot equipSlot;

    public override void Use()
    {
        base.Use();
        //Equip item in inventoryUI
        EquipmentManager.instance.Equip(this);

        //move out of inventory
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { melee, ranged, red, orange, yellow, green, blue, indigo, violet };