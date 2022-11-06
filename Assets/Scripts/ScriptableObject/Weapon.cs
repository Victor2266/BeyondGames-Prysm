using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName= "Inventory/Weapon")]
public class Weapon : Equipment
{

    public float ReachLength;
    public float DMG_Scaling;
    public int MaxDamage;
    public int MinDamage;
    public float DMGTextSize;
    public float activeTimeLimit;
    public float cooldownTime;
    public float XYSize;
    public float angle_offset;

    public Vector2 CapsuleColliderOffset;
    public Vector2 CapsuleColliderSize;
    public Vector2 SpriteOffset;

    public float handReachMultiplier;//where the hand mesh goes
    public GameObject projectileAttack;
    public float projectileOffset;

    public float thrustResetTime;
    public float thrustDashDist;
    public float thrustShortReach;//set this equal to the reach length for no recoil when shooting right click
    public bool projAsChild;
    public GameObject trail;
    public float soundPitch;
    public float movementDelay;
    public GameObject popSpawn;
    public bool oneSidedSwing;
    public bool swingOtherSide;

    public override void Use()
    {
        base.Use();
        
        //Swap out previous handheld item
    }
}
