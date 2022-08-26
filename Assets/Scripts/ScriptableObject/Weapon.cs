using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName= "Inventory/Weapon")]
public class Weapon : Equipment
{

    public float ReachLength;
    public float DMG_Scaling;
    public int MaxDamage;
    public float DMGTextSize;
    public float activeTimeLimit;
    public float cooldownTime;
    public float XYSize;
    public float angle_offset;

    public Vector2 CapsuleColliderOffset;
    public Vector2 CapsuleColliderSize;

    public float handReachMultiplier;
    public GameObject projectileAttack;
    public override void Use()
    {
        base.Use();
        
        //Swap out previous handheld item
    }
}
