using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : EventTrigger, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;
    public float[] stats;

    public InventoryUI.WeaponTypes type;
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        CallShowToolTipManager();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }

    public override void OnSelect(BaseEventData data)
    {
        CallShowToolTipManager();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        TooltipManager.Hide();
    }


    public void CallShowToolTipManager()
    {
        if (type == InventoryUI.WeaponTypes.Weapons)
            TooltipManager.Show(header, content, stats, "<color=yellow>Reach Length</color>\nSwinging Time\nReset Time\n<color=#F8B481>Damage Scaling</color>\n<color=#F8B481>Min Damage</color>\n<color=orange>Max Damage</color>\n<color=green>Special Cooldown</color>\n<color=red>Special Damage</color>");
        else if (type == InventoryUI.WeaponTypes.Spells)
            TooltipManager.Show(header, content, stats, "<color=green>Mana Cost</color>\n<color=orange>Damage</color>\nCooldown\n<color=green>Special Cost</color>\n<color=red>Special Damage</color>\nSpecial Cooldown");
        else
            TooltipManager.Show(header, content);
    }
}
