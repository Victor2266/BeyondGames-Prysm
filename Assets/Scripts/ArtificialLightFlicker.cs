// dnSpy decompiler from Assembly-CSharp.dll class: ArtificialLightFlicker
using System;
using UnityEngine;

public class ArtificialLightFlicker : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        float f = Time.time / this.duration * 2f * 3.14159274f;
        float num = Mathf.Cos(f) * this.range + this.range;
        this.lt.intensity = num + this.limit;
    }

    public float duration = 10f;

    public float range = 0.5f;

    public float limit;

    public Light lt;
}
