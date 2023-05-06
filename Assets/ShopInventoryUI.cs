using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/* This object updates the inventory UI. */

public class ShopInventoryUI : MonoBehaviour
{

	public Transform itemsParent;   // The parent object of all the items
	public GameObject inventoryUI;  // The entire UI
	public GameObject emptySlot;

	ShopInventory shop_inventory;    // Our current inventory
	Inventory player_inventory;    // Our current inventory

	InventorySlot[] slots;  // List of all the slots
	public Image[] Tabs = new Image[2];
	PlayerEntity player;
	public PlayerInput playerInput;


	public enum WeaponTypes { All, Weapons, Spells, Other };
	public String CurrentTab;

	private Color GreyedColor = new Color(1f, 1f, 1f, 0.5f);
	private Color SelectedColor = new Color(1f, 1f, 1f, 1f);

	private void OnEnable()
	{
		if (Inventory.instance != null)
		{
			player_inventory = Inventory.instance;
			player_inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback
		}
		shop_inventory = GetComponent<ShopInventory>();
	}
	void Start()
	{
		if (Inventory.instance != null)
		{
			player_inventory = Inventory.instance;
			player_inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback
		}
		shop_inventory = GetComponent<ShopInventory>();

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
		playerInput = player.GetComponent<PlayerInput>();
		// Populate our slots array
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	void Update()
	{

	}
	public void CloseInventory()
	{
		Resume();
		inventoryUI.SetActive(false);
	}
	private void Pause()
	{
		playerInput.actions.FindActionMap("UI").Enable();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		player.mousePointer.SetActive(false);
		player.GetComponent<PlayerController>().enabled = false;
		Time.timeScale = 0f;
		PauseMenuScript.isPaused = true;

	}

	private void Resume()
	{
		playerInput.SwitchCurrentActionMap("Player");
		Cursor.lockState = CursorLockMode.Confined;
		if (DialogManager.instance != null)
		{
			if (!DialogManager.instance.isDisplayingDialog)
				Cursor.visible = false;
		}
		else
		{
			Cursor.visible = false;
		}
		if (player.GetComponent<PlayerEntity>().weapon > 0)
		{
			player.mousePointer.SetActive(true);
		}
		else
		{
			player.mousePointer.SetActive(false);
		}
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
		if (itemsParent == null)
			return;
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		if(CurrentTab == "Shop")
        {
			while (slots.Length < shop_inventory.items.Count)
			{
				var newSlot = Instantiate(emptySlot, itemsParent);//new slot for adding item to UI
				slots = itemsParent.GetComponentsInChildren<InventorySlot>();
			}
			for (int i = 0; i < slots.Length; i++)
			{

				if (i < shop_inventory.items.Count)  // If there is an item to add
				{
					slots[i].AddItem(shop_inventory.items[i]);   // Add it
				}
				else
				{
					// Otherwise clear the slot
					slots[i].ClearSlot();
					Destroy(slots[i].gameObject);
				}
			}
		}
        else
        {
			while (slots.Length < player_inventory.items.Count)
			{
				var newSlot = Instantiate(emptySlot, itemsParent);//new slot for adding item to UI
				slots = itemsParent.GetComponentsInChildren<InventorySlot>();
			}
			for (int i = 0; i < slots.Length; i++)
			{

				if (i < player_inventory.items.Count)  // If there is an item to add
				{
					slots[i].AddItem(player_inventory.items[i]);   // Add it
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

	public void SetCurrentTab(int WeapType)
	{
		switch (WeapType)
		{
			case 1:
				CurrentTab = "Shop";
				Tabs[1].color = GreyedColor;
				Tabs[0].color = SelectedColor;
				break;
			case 2:
				CurrentTab = "Inventory";
				Tabs[0].color = GreyedColor;
				Tabs[1].color = SelectedColor;
				break;
		}
		Debug.Log("Shop tab: " + CurrentTab);
		UpdateUI();


	}
}