using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShaker : MonoBehaviour
{
    public void TakeDamageShake()
    {
        LeanTween.cancel(gameObject);

        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.moveX(gameObject, 10, 0.05f).setEaseInOutBounce());
        sequence.append(LeanTween.moveX(gameObject, 16, 0.12f).setEaseInOutBounce());
    }

}