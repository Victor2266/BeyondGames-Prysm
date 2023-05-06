using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

	public Image icon;          // Reference to the Icon image
	public TextMeshProUGUI textField;
	public TextMeshProUGUI textAmountField;
	//public Button removeButton; // Reference to the remove button

	public Item item;  // Current item in the slot
	[SerializeField] private TooltipTrigger tooltipTrigger;

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

		tooltipTrigger.header = newItem.name;
		tooltipTrigger.content = newItem.desc;
		tooltipTrigger.type = newItem.WeaponType;

        if (newItem.WeaponType == InventoryUI.WeaponTypes.Weapons)
        {
            Weapon newWeapon = (Weapon) newItem;
            projectileController projectile = newWeapon.projectileAttack.GetComponent<projectileController>();
			tooltipTrigger.stats = new float[] { newWeapon.ReachLength + newWeapon.XYSize * newWeapon.CapsuleColliderSize.y * 0.5f + newWeapon.CapsuleColliderOffset.magnitude, newWeapon.activeTimeLimit, newWeapon.cooldownTime, newWeapon.DMG_Scaling, newWeapon.MinDamage, newWeapon.MaxDamage, projectile.coolDownPeriod, projectile.DMG};
        }else if (newItem.WeaponType == InventoryUI.WeaponTypes.Spells)
        {
            Spell newSpell = (Spell)newItem;
            projectileController Spell = newSpell.SpellPrefab.GetComponent<projectileController>();
            projectileController chargedSpell = newSpell.ChargedSpellPrefab.GetComponent<projectileController>();
			tooltipTrigger.stats = new float[] {Spell.ManaCost, Spell.DMG, Spell.coolDownPeriod, chargedSpell.ManaCost, chargedSpell.DMG, chargedSpell.coolDownPeriod};
        }
        TooltipManager.Hide();
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
        TooltipManager.Hide();
    }

	// Called when the item is pressed
	public virtual void UseItem()
	{
		if (item != null)
		{
			item.Use();
		}
	}
}