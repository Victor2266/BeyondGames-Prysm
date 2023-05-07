using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogTrigger : DialogTrigger
{
    public Sprite characterSprite;

    public override void TriggerDialog()
    {
        DialogManager.instance.StartDialog(dialog, characterSprite);
        DialogManager.instance.maxSentences = dialog.sentences.Length;

    }
}
