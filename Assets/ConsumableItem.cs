﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableItem : Item
{
    public float healAmount;
    public int maxStacks;
    public override void Use()
    {
        base.Use();
        PlayerEntity playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
        if(playerEntity.currentHealth + healAmount <= playerEntity.MaxHealth)
        {
            playerEntity.setHealth(playerEntity.currentHealth + healAmount);
        }
        else
        {
            playerEntity.setHealth(playerEntity.MaxHealth);
        }
        RemoveFromInventory();

        //Drop ITem
    }
}
