using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Item icon
	public float iconRotation = 90f;
	public float iconSize = 1f;
	public bool isDefaultItem = false;

	public InventoryUI.WeaponTypes WeaponType = InventoryUI.WeaponTypes.Weapons;

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