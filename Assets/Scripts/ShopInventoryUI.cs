using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

/* This object updates the inventory UI. */

public class ShopInventoryUI : MonoBehaviour
{

	public Transform itemsParent;   // The parent object of all the items
	public GameObject inventoryUI;  // The entire UI
	public GameObject emptyInventorySlot;
	public GameObject emptyShopSlot;

	ShopInventory shop_inventory;    // Our current inventory
	Inventory player_inventory;    // Our current inventory

	ShopSlot[] shop_slots;  // List of all the slots
	InventorySlot[] inventory_slots;  // List of all the slots

	public Image[] Tabs = new Image[2];
	PlayerEntity player;

	public SelectedItemSlot selectedItemSlot;

	public enum WeaponTypes { All, Weapons, Spells, Other };
	public String CurrentTab;

	private Color GreyedColor = new Color(1f, 1f, 1f, 0.5f);
	private Color SelectedColor = new Color(1f, 1f, 1f, 1f);

	public TextMeshProUGUI SoulsText;
	public TextMeshProUGUI priceText;
	public TextMeshProUGUI descText;

	public TextMeshProUGUI errorMSG;
	public TextMeshProUGUI confirmMSG;

	private void OnEnable()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();

		if (Inventory.instance != null)
		{
			player_inventory = Inventory.instance;
			player_inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback
		}
		shop_inventory = GetComponent<ShopInventory>();
		selectedItemSlot.ClearSlot();
		SoulsText.text = "Souls:" + player.Souls.ToString();
		priceText.text = "Price: " + "0";
		descText.text = "";

		shop_slots = itemsParent.GetComponentsInChildren<ShopSlot>(); //keep track of shop slots and inventory slots separately
		inventory_slots = itemsParent.GetComponentsInChildren<InventorySlot>();
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
		// Populate our slots array
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

		shop_slots = itemsParent.GetComponentsInChildren<ShopSlot>();
		inventory_slots = itemsParent.GetComponentsInChildren<InventorySlot>();

		if (CurrentTab == "Shop")
        {
			foreach(InventorySlot IS in inventory_slots)
            {
				Destroy(IS.gameObject);
            }

			while (shop_slots.Length < shop_inventory.items.Count)
			{
				var newSlot = Instantiate(emptyShopSlot, itemsParent);//new slot for adding item to UI
				newSlot.GetComponent<ShopSlot>().setShopUI(this);
				shop_slots = itemsParent.GetComponentsInChildren<ShopSlot>();
			}
			for (int i = 0; i < shop_slots.Length; i++)
			{

				if (i < shop_inventory.items.Count)  // If there is an item to add
				{
					shop_slots[i].AddItem(shop_inventory.items[i]);   // Add it

					if (!(shop_slots[i].item is ConsumableItem))//so its a weapon a spell or a note
					{
						if (Inventory.instance.CheckForContains(shop_inventory.items[i]) || EquipmentManager.instance.Contains(shop_inventory.items[i]))//check if contains works
						{
							// Otherwise clear the slot
							shop_slots[i].ClearSlot();
							Destroy(shop_slots[i].gameObject);
						}
					}
				}
				else
				{
					// Otherwise clear the slot
					shop_slots[i].ClearSlot();
					Destroy(shop_slots[i].gameObject);
				}
			}
		}
        else
        {
			foreach (ShopSlot SS in shop_slots)
			{
				Destroy(SS.gameObject);
			}

			while (inventory_slots.Length < player_inventory.items.Count)
			{
				var newSlot = Instantiate(emptyInventorySlot, itemsParent);//new slot for adding item to UI
				inventory_slots = itemsParent.GetComponentsInChildren<InventorySlot>();
			}
			for (int i = 0; i < inventory_slots.Length; i++)
			{

				if (i < player_inventory.items.Count)  // If there is an item to add
				{
					inventory_slots[i].AddItem(player_inventory.items[i]);   // Add it
				}
				else
				{
					// Otherwise clear the slot
					inventory_slots[i].ClearSlot();
					Destroy(inventory_slots[i].gameObject);
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
		priceText.text = "Price: " + item.cost.ToString();
		descText.text = item.desc;
	}

	public void Purchase()
    {
		bool wasPickedUp;
		if(selectedItemSlot.item == null)
        {
			return;
        }
		Debug.Log(player.Souls - selectedItemSlot.item.cost >= 0);
		if (player.Souls - selectedItemSlot.item.cost >= 0 && ( (selectedItemSlot.item is ConsumableItem) ? (Inventory.instance.CountDuplicates(selectedItemSlot.item) < ((ConsumableItem)selectedItemSlot.item).maxStacks) : true))
		{
			player.setSouls(player.Souls - selectedItemSlot.item.cost);
			SoulsText.text = "Souls:" + player.Souls.ToString();

			wasPickedUp = Inventory.instance.Add(selectedItemSlot.item);    // Add to inventory

			confirmMSG.text = "Added " + selectedItemSlot.item.name + " to Inventory (" + Inventory.instance.CountDuplicates(selectedItemSlot.item) + ")";
			confirmMSG.transform.parent.gameObject.SetActive(true);
		}
		else
		{
			//DISPLAY NOT ABLE TO PURCHASE ERROR
			if (player.Souls - selectedItemSlot.item.cost < 0)
			{
				Debug.Log("COULD NOT PURCHASE: " + selectedItemSlot.item.name + ": Not enough souls");
				errorMSG.color = Color.red;
				errorMSG.text = "YOU DON'T HAVE ENOUGH SOULS";
				errorMSG.transform.parent.gameObject.SetActive(true);
			}
			if ((selectedItemSlot.item is ConsumableItem) ? Inventory.instance.CountDuplicates(selectedItemSlot.item) >= ((ConsumableItem)selectedItemSlot.item).maxStacks : false)
			{
				Debug.Log("COULD NOT PURCHASE: " + selectedItemSlot.item.name + " You have too many");
				errorMSG.color = Color.green;
				errorMSG.text = "YOU ALREADY HAVE " + ((ConsumableItem)selectedItemSlot.item).maxStacks + " " + selectedItemSlot.item.name.ToUpper() + "(s)";
				errorMSG.transform.parent.gameObject.SetActive(true);
			}
		}
		

		UpdateUI();
	}

}