using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName= "Inventory/Weapon")]
public class Weapon : Equipment
{

    public float ReachLength;
    private int DMG;
    public float DMG_Scaling;
    public int MaxDamage;
    public float DMGTextSize;
    public float activeTimeLimit;
    public float cooldownTime;

    public override void Use()
    {
        base.Use();
        
        //Swap out previous handheld item
    }
}
