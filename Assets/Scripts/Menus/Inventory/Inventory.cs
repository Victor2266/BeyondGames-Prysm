using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	#region Singleton

	public static Inventory instance;

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of Inventory found!");
			return;
		}

		instance = this;
	}

    #endregion

    private void Start()
    {
		items = SaveSystem.LoadInventory();
		if (items == null)
        {
			items = new List<Item>();
		}
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
    // Callback which is triggered when
    // an item gets added/removed.
    public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public int space = 99;  // Amount of slots in inventory

	// Current list of items in inventory
	public List<Item> items = new List<Item>();

	// Add a new item. If there is enough room we
	// return true. Else we return false.
	public bool Add(Item item)
	{
		// Don't do anything if it's a default item
		if (!item.isDefaultItem)
		{
			// Check if out of space
			if (items.Count >= space)
			{
				Debug.Log("Not enough room.");
				return false;
			}

			if(item is ConsumableItem)
            {
				int duplicates = 0;
				foreach (Item currentItem in items)
				{
					if (currentItem.name == item.name)//check for duplicates in inventory
					{
						duplicates++;
					}
				}

				if (duplicates < ((ConsumableItem)item).maxStacks)
                {
					items.Add(item);    // Add item to list

					// Trigger callback
					if (onItemChangedCallback != null)
						onItemChangedCallback.Invoke();
				}
			}

			foreach(Item currentItem in items)
            {
				if (currentItem.name == item.name)//check for duplicates in inventory
                {
					return true;
                }
            }
			foreach (Equipment currentEquips in EquipmentManager.instance.getCurrentEquipment())
			{
				if(currentEquips != null)
                {
					if (currentEquips.name == item.name)//check for duplicates in equipped items
					{
						return true;
					}
				}
			}
			
			
			items.Add(item);    // Add item to list

			// Trigger callback
			if (onItemChangedCallback != null)
				onItemChangedCallback.Invoke();
		}

		return true;
	}

	// Remove an item
	public void Remove(Item item)
	{
		items.Remove(item);     // Remove item from list

		// Trigger callback
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}

}