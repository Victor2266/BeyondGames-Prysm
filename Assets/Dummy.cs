using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float totalDamage;
    //public DamageBar dmgBar;
    public void TakeDamage(float amount)
    {
        totalDamage += amount;
        Debug.Log(totalDamage);
    }
}
