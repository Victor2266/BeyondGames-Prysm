using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public EquipmentSlot equipSlot;
    public Color color;

    public enum ElementType { Red, Orange, Yellow, Green, Blue, Indigo, Violet, Black, White, None }
    public ElementType ElementalType = ElementType.None;

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