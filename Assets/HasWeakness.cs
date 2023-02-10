using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HasWeakness : MonoBehaviour
{
    public Equipment.ElementType WeaknessTo;
    public float WeaknessMultiplier = 1;
    public Equipment.ElementType ImmunityTo;
    public float ImmunityMultiplier = 1;

    public abstract void TakeDamage(float DMG);
}
