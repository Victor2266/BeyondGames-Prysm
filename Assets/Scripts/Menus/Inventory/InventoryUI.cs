using System;
using UnityEngine;

/* This object updates the inventory UI. */

public class InventoryUI : MonoBehaviour
{

	public Transform itemsParent;   // The parent object of all the items
	public GameObject inventoryUI;  // The entire UI
	public GameObject emptySlot;

	Inventory inventory;    // Our current inventory

	InventorySlot[] slots;  // List of all the slots

	PlayerEntity player;
	void Start()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();

		// Populate our slots array
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	void Update()
	{
		// Check to see if we should open/close the inventory
		if (Input.GetButtonDown("Inventory"))
		{
				if (PauseMenuScript.isPaused)
				{
					Resume();
				}
				else
				{
					Pause();
				}
			
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}
	}

    private void Pause()
    {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		player.mousePointer.SetActive(false);
		player.GetComponent<PlayerController>().enabled = false;
		Time.timeScale = 0f;
		PauseMenuScript.isPaused = true;
	}

    private void Resume()
    {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
		player.mousePointer.SetActive(true);
		player.GetComponent<PlayerController>().enabled = true;
		Time.timeScale = 1f;
		PauseMenuScript.isPaused = false;
	}

    // Update the inventory UI by:
    //		- Adding items
    //		- Clearing empty slots
    // This is called using a delegate on the Inventory.
    void UpdateUI()
	{
		// Loop through all the slots
		var newSlot = Instantiate(emptySlot, itemsParent);//new slot for adding item to UI
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventory.items.Count)  // If there is an item to add
			{
				
				slots[i].AddItem(inventory.items[i]);   // Add it
			}
			else
			{
				// Otherwise clear the slot
				slots[i].ClearSlot();
				Destroy(slots[i].gameObject);
			}
		}
	}
}