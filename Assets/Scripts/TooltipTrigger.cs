using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;
    public float[] stats;

    public InventoryUI.WeaponTypes type;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        CallShowToolTipManager();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }

    private void CallShowToolTipManager()
    {
        if (type == InventoryUI.WeaponTypes.Weapons)
            TooltipManager.Show(header, content, stats, "<color=yellow>Reach Length</color>\nSwinging Time\nReset Time\n<color=#F8B481>Damage Scaling</color>\n<color=#F8B481>Min Damage</color>\n<color=orange>Max Damage</color>\n<color=green>Special Cooldown</color>\n<color=red>Special Damage</color>");
        else if (type == InventoryUI.WeaponTypes.Spells)
            TooltipManager.Show(header, content, stats, "<color=orange>Damage</color>\nCooldown\n<color=red>Special Damage</color>\nSpecial Cooldown");
        else
            TooltipManager.Show(header, content);
    }
}
