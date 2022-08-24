using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

	public Image icon;          // Reference to the Icon image
	public TextMeshProUGUI textField;
	//public Button removeButton; // Reference to the remove button

	public Item item;  // Current item in the slot

	// Add item to the slot
	public void AddItem(Item newItem)
	{
		item = newItem;
		textField.text = item.name;
		icon.sprite = item.icon;
		icon.enabled = true;
		icon.rectTransform.eulerAngles = new Vector3(icon.rectTransform.rotation.x, icon.rectTransform.rotation.y, item.iconRotation);
		icon.rectTransform.localScale = new Vector3(item.iconSize, item.iconSize, 1f);
		icon.SetNativeSize();
		//removeButton.interactable = true;
	}

	// Clear the slot
	public void ClearSlot()
	{
		item = null;

		icon.sprite = null;
		icon.enabled = false;
		//removeButton.interactable = false;
	}

	// Called when the remove button is pressed
	public void OnRemoveButton()
	{
		Inventory.instance.Remove(item);
	}

	// Called when the item is pressed
	public void UseItem()
	{
		if (item != null)
		{
			item.Use();
		}
	}
}