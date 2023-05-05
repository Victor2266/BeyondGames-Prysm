using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryShopStarter : InGameDialogStarter
{
    public GameObject shopCanvas;
    public override void ResetDialog()//the close button also usees this function
    {
        if (distance <= radius)
        {
            shopCanvas.SetActive(true);
            DialogManager.instance.isDisplayingDialog = true;
        }
        else
        {
            closeButton();
        }

    }

    public void closeButton()
    {
        DialogManager.instance.isDisplayingDialog = true;
        shopCanvas.SetActive(false);
        base.ResetDialog();
    }
}
