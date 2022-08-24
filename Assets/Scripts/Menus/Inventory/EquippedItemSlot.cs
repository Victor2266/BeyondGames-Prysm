﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItemSlot : InventorySlot
{
	public EquipmentSlot slot;
    private void Start()
    {
		EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}
	private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
	{
		if(newItem != null)
        {
			if (newItem.equipSlot == slot)
			{
				AddItem(newItem);
			}
		}
		else if(oldItem.equipSlot == slot)
        {
			ClearSlot();
        }
    }
    public void UnequipItem(int slotIndex)//slots 0-9 for melee to violet
	{
		if (item != null)
		{
			EquipmentManager.instance.Unequip(slotIndex);
		}
	}
	private new void AddItem(Item newItem)
	{
		item = newItem;

		icon.sprite = item.icon;
		icon.enabled = true;
		//removeButton.interactable = true;
	}
}
