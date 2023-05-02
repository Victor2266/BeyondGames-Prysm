using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/* This object updates the inventory UI. */

public class InventoryUI : MonoBehaviour
{

	public Transform itemsParent;   // The parent object of all the items
	public GameObject inventoryUI;  // The entire UI
	public GameObject emptySlot;

	Inventory inventory;    // Our current inventory

	InventorySlot[] slots;  // List of all the slots
	public Image[] Tabs = new Image[3];
	PlayerEntity player;
	public PlayerInput playerInput;

	public GameObject pauseMenu;

	public enum WeaponTypes { All, Weapons, Spells };
	public WeaponTypes CurrentTab;

	private Color GreyedColor = new Color(1f, 1f, 1f, 0.5f);
	private Color SelectedColor = new Color(1f, 1f, 1f, 1f);

    private void OnEnable()
    {
		if(Inventory.instance != null)
        {
			inventory = Inventory.instance;
			inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback
		}
	}
    void Start()
	{
		if (Inventory.instance != null)
		{
			inventory = Inventory.instance;
			inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback
		}
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
		// Populate our slots array
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	void Update()
	{
		// Check to see if we should open/close the inventory
		if (playerInput.actions["Inventory"].WasPressedThisFrame())
		{
			if (PauseMenuScript.isPaused)
			{
				if (inventoryUI.activeSelf == true)
                {
					Resume();
					inventoryUI.SetActive(!inventoryUI.activeSelf);
				}

			}
			else
			{
				Pause();
				inventoryUI.SetActive(!inventoryUI.activeSelf);
			}


            TooltipManager.Hide();
		}
		else if (playerInput.actions["Pause"].WasPressedThisFrame() && inventoryUI.activeSelf == true)
        {
            //CloseInventory();

            TooltipManager.Hide();
        }
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
		Cursor.visible = false;
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
		while (slots.Length < inventory.items.Count)
		{
			var newSlot = Instantiate(emptySlot, itemsParent);//new slot for adding item to UI
			slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		}
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

			if (CurrentTab == WeaponTypes.Weapons)
			{
				if (inventory.items[i].WeaponType != WeaponTypes.Weapons)
				{
					// Otherwise clear the slot
					slots[i].ClearSlot();
					Destroy(slots[i].gameObject);
				}
			}
			else if (CurrentTab == WeaponTypes.Spells)
			{
				if (inventory.items[i].WeaponType != WeaponTypes.Spells)
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
				CurrentTab = WeaponTypes.Weapons;
				Tabs[0].color = GreyedColor;
				Tabs[2].color = GreyedColor;
				Tabs[1].color = SelectedColor;
				break;
			case 2:
				CurrentTab = WeaponTypes.Spells;
				Tabs[0].color = GreyedColor;
				Tabs[1].color = GreyedColor;
				Tabs[2].color = SelectedColor;
				break;
			default:
				CurrentTab = WeaponTypes.All;
				Tabs[1].color = GreyedColor;
				Tabs[2].color = GreyedColor;
				Tabs[0].color = SelectedColor;
				break;
		}
		Debug.Log("Current tab: " + CurrentTab);
		UpdateUI();
		
	
	}
}