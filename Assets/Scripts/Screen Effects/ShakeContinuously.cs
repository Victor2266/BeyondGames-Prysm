using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EZCameraShake;

public class ShakeContinuously : MonoBehaviour
{
    [SerializeField]
    float ShakeMagnitude = 0f;
    [SerializeField]
    float ShakeRoughness = 0f;
    [SerializeField]
    float ShakeFadeIn = 0f;
    [SerializeField]
    float ShakeFadeOut = 0f;

    // Use this for initialization
    private void Start()
    {
        CameraShaker.Instance.ShakeOnce(ShakeMagnitude, ShakeRoughness, ShakeFadeIn, ShakeFadeOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
