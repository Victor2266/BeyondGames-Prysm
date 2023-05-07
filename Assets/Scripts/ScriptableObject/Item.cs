using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Item icon
	public float iconRotation = 90f;
	public float iconSize = 1f;
	public bool isDefaultItem = false; // This means taht the player starts with the item so the inventory doesn't need to do anything to add it in the addItem() method
	public int cost;// this is for purchases in a store, not all items will be sold

	public InventoryUI.WeaponTypes WeaponType = InventoryUI.WeaponTypes.Weapons;
    [TextArea(3,10)]
    public string desc;
	public bool hideDesc;
	[TextArea(3, 10)]
	public string hidden_desc;
	// Called when the item is pressed in the inventory
	public virtual void Use()
	{
		// Use the item
		// Something might happen

		Debug.Log("Using " + name);
	}

	public void RemoveFromInventory()
	{
		Inventory.instance.Remove(this);

	}

}