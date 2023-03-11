using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/DoubleStateWeapon")]
public class DoubleStateWeapon : Weapon
{
    public float DMG_Scaling2;
    public int MaxDamage2, MinDamage2;
    public float activeTimeLimit2;
    public float cooldownTime2;

    public float movementDelay2;//movement delay 2 is used for alternate right click abilities,
    public GameObject popSpawn2;

    public ElementType ElementalType2 = ElementType.None;


    public override void Use()
    {
        base.Use();

        //Swap out previous handheld item
    }
}
