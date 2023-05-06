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

	InventorySlot[] slots;  // List of all the slots
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
				slots = itemsParent.GetComponentsInChildren<ShopSlot>();
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

				if(!(slots[i].item is ConsumableItem))//so its a weapon a spell or a note
                {
                    if (Inventory.instance.items.Contains(slots[i].item))//check if contains works
                    {
						// Otherwise clear the slot
						slots[i].ClearSlot();
						Destroy(slots[i].gameObject);
					}
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

		if(player.Souls - selectedItemSlot.item.cost >= 0 && Inventory.instance.CountDuplicates(selectedItemSlot.item) < ((ConsumableItem)selectedItemSlot.item).maxStacks)
        {
			player.setSouls(player.Souls - selectedItemSlot.item.cost);
			SoulsText.text = "Souls:" + player.Souls.ToString();

			wasPickedUp = Inventory.instance.Add(selectedItemSlot.item);    // Add to inventory

			confirmMSG.text = "Added " + selectedItemSlot.item.name + " to Inventory";
			confirmMSG.transform.parent.gameObject.SetActive(true);
		}
        else
        {
			//DISPLAY NOT ABLE TO PURCHASE ERROR
			if(player.Souls - selectedItemSlot.item.cost < 0)
			{
				Debug.Log("COULD NOT PURCHASE: " + selectedItemSlot.item.name + ": Not enough souls");
				errorMSG.color = Color.red;
				errorMSG.text = "YOU DON'T HAVE ENOUGH SOULS";
				errorMSG.transform.parent.gameObject.SetActive(true);
			}
			if(Inventory.instance.CountDuplicates(selectedItemSlot.item) >= ((ConsumableItem)selectedItemSlot.item).maxStacks)
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