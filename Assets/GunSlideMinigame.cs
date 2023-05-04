using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSlideMinigame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogManager.instance.index == 12)//MAG IS IN MAG WELL
        {
            DialogManager.instance.index++;
        }
    }
}
