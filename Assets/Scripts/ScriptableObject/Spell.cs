using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Spell")]
public class Spell : Equipment
{
    public GameObject SpellPrefab;
    public GameObject ChargedSpellPrefab;

    public override void Use()
    {
        base.Use();

        //Swap out previous handheld item
    }
}
