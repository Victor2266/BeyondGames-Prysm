using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BerryShopStarter : InGameDialogStarter
{
    public GameObject shopCanvas;
    public override void ResetDialog()//the close button also usees this function
    {
        if (playerInput.actions["Pause"].WasPressedThisFrame() || playerInput.actions["Inventory"].WasPressedThisFrame())
        {
            closeButton();
        }
        else if (distance <= radius)
        {
            shopCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            DialogManager.instance.isDisplayingDialog = true;
            Cursor.visible = true;
        }
        else if(hasInteracted)
        {
            closeButton();
        }

    }

    public void closeButton()
    {
        TooltipManager.Hide();
        DialogManager.instance.isDisplayingDialog = false;

        base.ResetDialog();

        shopCanvas.SetActive(false);

    }
}
