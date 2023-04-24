using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OscillateColorWhiteness : MonoBehaviour
{
    public float Speed;
    public float centre;
    public float Amplitude;

    // Update is called once per frame
    void Update()
    {
        float num = (centre + Mathf.Cos(Time.time * Speed) * Amplitude) / 2f;
        GetComponent<Image>().color = new Color(num, num, num, GetComponent<Image>().color.a);
    }
}
