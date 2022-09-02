using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveEquipment
{
    public List<string> items;
    public SaveEquipment(List<string> newitems)
    {
        items = newitems;
    }
}
