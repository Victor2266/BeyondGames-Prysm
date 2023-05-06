﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TweenToTransparent : MonoBehaviour
{

    public Image image;

    public float startingAlpha;

    public float tweenTime;

    // Start is called before the first frame update
    void onEnable()
    {
        image.color = new Vector4(image.color.r, image.color.g, image.color.b, startingAlpha);
        LeanTween.value(startingAlpha, 0f, tweenTime).setOnUpdate((float val) => { image.color = new Color(image.color.r, image.color.g, image.color.b, val); }).setEaseOutExpo().setOnComplete(Disable);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
