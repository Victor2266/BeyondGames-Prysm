using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBar : MonoBehaviour
{
    public RectTransform redBar;
    public void UpdateBar(float newDMG, float maxDMG)
    {
        if (newDMG >= 0)
        {
            LeanTween.cancel(redBar);
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.scale(redBar, new Vector3(newDMG / maxDMG, 1f, 1f), 0.1f));
            sequence.append(LeanTween.scale(redBar, new Vector3(0f, 1f, 1f), 0.5f));
        }
    }

}
