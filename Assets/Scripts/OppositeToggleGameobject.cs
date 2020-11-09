using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeToggleGameobject : MonoBehaviour
{
    public List<GameObject> UIObject = new List<GameObject>();

    public void ToggleActive(bool active)
    {
        foreach(GameObject obj in UIObject)
        {
            obj.SetActive(!active);
        }
    }
}
