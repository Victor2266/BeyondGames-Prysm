using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dpsText : MonoBehaviour
{
    public float totalDMG;
    public void TakeDamage(float amount)
    {
        LeanTween.cancelAll();
        totalDMG += amount;
        LeanTween.value(gameObject, totalDMG, 0f, 1f);
    }
}
