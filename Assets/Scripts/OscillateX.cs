using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateX : MonoBehaviour
{
    public float startRot;
    public float Speed;
    public float HeightAmp;
    public bool isChild;

    // Update is called once per frame
    void Update()
    {
        if (!isChild)
            this.transform.eulerAngles = new Vector3(startRot + Mathf.Cos(Time.time * Speed) * HeightAmp, transform.eulerAngles.y, transform.eulerAngles.z);
        else
            this.transform.localEulerAngles = new Vector3( startRot + Mathf.Cos(Time.time * Speed) * HeightAmp, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
