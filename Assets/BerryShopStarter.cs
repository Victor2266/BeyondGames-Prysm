using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BerryShopStarter : InGameDialogStarter
{
    public GameObject shopCanvas;
    public override void ResetDialog()//the close button also usees this function
    {
        if (distance <= radius)
        {
            shopCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            //set dialog box tag to untagged
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

        //set dialog box tag to DefaultOption2
        shopCanvas.SetActive(false);
        base.ResetDialog();
    }
}
