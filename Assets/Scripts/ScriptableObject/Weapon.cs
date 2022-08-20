using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public float ReachLength;
    private int DMG;
    public float DMG_Scaling;
    public int MaxDamage;
    public float DMGTextSize;
    public float activeTimeLimit;
    public float cooldownTime;
}
