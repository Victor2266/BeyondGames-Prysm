using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaColourFadeInForEndScreen : MonoBehaviour
{
    RawImage rawImage;
    SpriteRenderer sprtRend;

    public bool UseRawImage = true;
    public bool UseSpriteRend;
    public bool ResetAlphaToZeroOnAwake;

    public float ticks;
    public float tickLimit;
    private float timeend;
    private float endValue;
    private float startValue;

    // Start is called before the first frame update
    void OnEnable()
    {
        rawImage = GetComponent<RawImage>();
        sprtRend = GetComponent<SpriteRenderer>();

        if (ResetAlphaToZeroOnAwake)
        {
            if (UseRawImage)
            {
                rawImage.color = new Vector4(rawImage.color.r, rawImage.color.g, rawImage.color.b, 00);
            }
            else if (UseSpriteRend)
            {
                sprtRend.color = new Vector4(sprtRend.color.r, sprtRend.color.g, sprtRend.color.b, 0f);
            }
        }

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
            if (UseRawImage)
            {
                if (rawImage.color.a < 1)
                {
                    rawImage.color = new Vector4(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1 * ((ticks - startValue) / (tickLimit)));
                }
            }
            else if (UseSpriteRend)
            {
                if (sprtRend.color.a < 1)
                {
                    sprtRend.color = new Vector4(sprtRend.color.r, sprtRend.color.g, sprtRend.color.b, 1 * ((ticks - startValue) / (tickLimit)));
                }
            }
        }
        
    }
}
