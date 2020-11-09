using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaColourFadeInForEndScreen : MonoBehaviour
{
    RawImage rawImage;

    public float ticks;
    public float tickLimit;
    private float timeend;
    private float endValue;
    private float startValue;

    // Start is called before the first frame update
    void Awake()
    {
        rawImage = GetComponent<RawImage>();

        ticks = Time.time;
        timeend = Time.time + tickLimit;
        endValue = timeend - ticks;
        startValue = ticks;
    }

    // Update is called once per frame
    void Update()
    {
        if (ticks < timeend)
        {
            ticks = Time.time;

            if (rawImage.color.a < 1)
            {
                rawImage.color = new Vector4(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1 * ((ticks - startValue) / (tickLimit)));
            }
        }
        
    }
}
