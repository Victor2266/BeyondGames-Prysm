using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;

    public virtual void TriggerDialog()
    {
        DialogManager.instance.StartDialog(dialog);
        DialogManager.instance.maxSentences = dialog.sentences.Length;

    }
}
