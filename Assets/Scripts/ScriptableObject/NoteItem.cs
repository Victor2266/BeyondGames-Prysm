using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Inventory/Note")]
public class NoteItem : Item
{
    public GameObject DropItem;

    public override void Use()
    {
        base.Use();
        Instantiate(DropItem, GameObject.FindGameObjectWithTag("Player").transform.position, GameObject.FindGameObjectWithTag("Player").transform.rotation);
        RemoveFromInventory();

        //Drop ITem
    }
}
