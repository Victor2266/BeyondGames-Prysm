using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName= "Inventory/Weapon")]
public class Weapon : Equipment
{
    public string AttackName;
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
    public float movementDelay;//movement delay 2 is used for alternate right click abilities,
    public bool speedUpWithDamage;
    public float minimumMovementDelay;//for speed up weapons
    public GameObject popSpawn;
    public GameObject dmgTextObj; 
    public bool oneSidedSwing;
    public bool swingOtherSide;
    public bool usesPhysicalCollider;
    //Physical colliders are NOT good for pole type weapons with full length colliders(-Bo staff), they are not good for colliders that are close to the player body(-Cheap Sword).
    //Physical colliders will hold back enemies with the press of a button which helps create distance. (+Scythe weapons)(+Longsword)
    //The knock back from the pop effect will be virtually removed by the collider holding the enemy against the ground during an overhead swing (-Slaughter sword)
        //so don't use a physical collider for weapons with big knockback pops.

    public ElementType ElementalType = ElementType.None;

    public leftClickStrat leftClickStrategy = leftClickStrat.Default;
    public rightClickStrat rightClickStrategy = rightClickStrat.Default;
    public float continuousDashAccelMagnitude;
    public float continuousDashAccelTime;

    public override void Use()
    {
        base.Use();
        
        //Swap out previous handheld item
    }
}
