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
	public GameObject emptyInventorySlot;
	public GameObject emptyShopSlot;

	ShopInventory shop_inventory;    // Our current inventory
	Inventory player_inventory;    // Our current inventory

	InventorySlot[] slots;  // List of all the slots
	public Image[] Tabs = new Image[2];
	PlayerEntity player;
	public PlayerInput playerInput;

	public SelectedItemSlot selectedItemSlot;

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
		selectedItemSlot.ClearSlot();
		UpdateUI();
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
				var newSlot = Instantiate(emptyShopSlot, itemsParent);//new slot for adding item to UI
				newSlot.GetComponent<ShopSlot>().setShopUI(this);
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
				var newSlot = Instantiate(emptyInventorySlot, itemsParent);//new slot for adding item to UI
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

    public void SelectItem(Item item)
    {
		Debug.Log("Select for shop: " + item.name);
		selectedItemSlot.AddItem(item);
	}

	public void Purchase()
    {
		bool wasPickedUp = Inventory.instance.Add(selectedItemSlot.item);    // Add to inventory

	}
}