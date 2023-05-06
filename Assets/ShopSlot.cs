using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : InventorySlot
{
	public ShopInventoryUI ShopUI;//set on instantiate

	public void setShopUI(ShopInventoryUI s)
    {
		ShopUI = s;
    }

	public override void UseItem()
	{
		if (item != null)
		{
			ShopUI.SelectItem(item);
		}
	}
}
