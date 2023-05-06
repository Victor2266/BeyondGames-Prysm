using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : InventorySlot
{
	public override void UseItem()
	{
		if (item != null)
		{
			Debug.Log("Select for shop: " + item.name);
		}
	}
}
