using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour//rewrote for canvas world space health bars including tweening, this breaks the old implementation for skeletons
{
    public RectTransform redBar;
    public RectTransform whiteBar;

    public void UpdateHealthBar(float newHealth, float maxHealth)
    {
        if (newHealth >= 0)
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.scale(redBar, new Vector3(newHealth / maxHealth, 1f, 1f), 0.1f));
            sequence.append(LeanTween.scale(whiteBar, new Vector3(newHealth / maxHealth, 1f, 1f), 1.5f));
        }
        else
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.scale(redBar, new Vector3(0f, 1f, 1f), 0.1f));
            sequence.append(LeanTween.scale(whiteBar, new Vector3(0f, 1f, 1f), 1.5f));
        }
    }
    
}