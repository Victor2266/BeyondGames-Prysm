using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterSeconds : MonoBehaviour
{

    public float DeactivateAfterThisAmount;
    private float timeStamp;
    // Start is called before the first frame update
    void OnEnable()
    {
        timeStamp = Time.time + DeactivateAfterThisAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeStamp)
        {
            gameObject.SetActive(false);
        }
    }
}
