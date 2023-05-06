using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItemSlot : InventorySlot
{
	public GameObject outline;
	public override void AddItem(Item newItem)
	{
		item = newItem;

		icon.sprite = item.icon;
		icon.enabled = true;
		//removeButton.interactable = true;

		icon.rectTransform.eulerAngles = new Vector3(icon.rectTransform.rotation.x, icon.rectTransform.rotation.y, item.iconRotation);

		icon.rectTransform.localScale = new Vector3(item.iconSize, item.iconSize, 1f);
		icon.SetNativeSize();
		
		outline.SetActive(true);
	}

	public void ClearSlot()
	{
		item = null;

		icon.sprite = null;
		icon.enabled = false;

		outline.SetActive(false);
		//removeButton.interactable = false;
	}
}
