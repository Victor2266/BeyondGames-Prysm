using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecificActivation : MonoBehaviour
{
    public RuntimePlatform platform;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == platform) {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


}
