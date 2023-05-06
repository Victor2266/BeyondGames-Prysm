﻿using System.Collections;
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
            //set dialog box tag to untagged
            DialogManager.instance.isDisplayingDialog = true;
        }
        else
        {
            TooltipManager.Hide();
            closeButton();
        }

    }

    public void closeButton()
    {
        DialogManager.instance.isDisplayingDialog = false;

        //set dialog box tag to DefaultOption2
        shopCanvas.SetActive(false);
        base.ResetDialog();
    }
}
