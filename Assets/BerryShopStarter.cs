using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryShopStarter : InGameDialogStarter
{
    public override void ResetDialog()//the close button also usees this function
    {
        if (distance <= radius)
        {
            Debug.Log("opensop");
        }
        base.ResetDialog();
    }
}
