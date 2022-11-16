using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToTransparent : MonoBehaviour
{
    RawImage rawImage;
    public Image image;
    SpriteRenderer sprtRend;

    public bool UseRawImage = true;
    public bool UseSpriteRend;
    public bool UseImage;
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
            else if (UseImage)
            {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0f);
            }
        }

        ticks = Time.time;
        timeend = Time.time + tickLimit;
        endValue = timeend - ticks;
        startValue = ticks;
    }
    public float alpha;
    // Update is called once per frame
    void Update()
    {
        if (ticks < timeend)
        {
            alpha = 1f - ((ticks - startValue) / (tickLimit));
            ticks = Time.time;
            if (UseRawImage)
            {
                if (rawImage.color.a <= 1)
                {
                    rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);
                }
            }
            else if (UseSpriteRend)
            {
                if (sprtRend.color.a <= 1)
                {
                    sprtRend.color = new Color(sprtRend.color.r, sprtRend.color.g, sprtRend.color.b, alpha);
                }
            }
            else if (UseImage)
            {
                if (image.color.a <= 1)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
