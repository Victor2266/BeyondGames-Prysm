using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public EquipmentSlot equipSlot;
    public Color color;

    public enum ElementType { Red, Orange, Yellow, Green, Blue, Indigo, Violet, Black, White, None, All }

    public enum leftClickStrat { Default, Anchor, Dashing }//anchor, dashing, mana drain, etc
    public new enum rightClickStrat { Default, DoubleStateDrains, DoubleStateCost, Dashing }
    //DoubleStateDrains (DoubleStateWeapon only): Uses the mana cost from the projectile controller script to drain mana
    //DoubleStateCost (DoubleStateWeapon only): Spawns proj with upfront cost

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