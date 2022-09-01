using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateY : MonoBehaviour
{
    public float startRot;
    public float Speed;
    public float HeightAmp;
    public bool isChild;

    // Update is called once per frame
    void Update()
    {
        if (!isChild)
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, startRot + Mathf.Cos(Time.time * Speed) * HeightAmp, transform.eulerAngles.z);
        else
            this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, startRot + Mathf.Cos(Time.time * Speed) * HeightAmp, transform.localEulerAngles.z);
    }
}
