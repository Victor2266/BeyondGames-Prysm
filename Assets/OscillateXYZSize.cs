using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateXYZSize : MonoBehaviour
{
    public float startSize;
    public float Speed;
    public float Amplitude;

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(startSize + Mathf.Cos(Time.time * Speed) * Amplitude, startSize + Mathf.Cos(Time.time * Speed) * Amplitude, startSize + Mathf.Cos(Time.time * Speed) * Amplitude);
    }
}
