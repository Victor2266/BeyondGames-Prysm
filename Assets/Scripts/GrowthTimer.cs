using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthTimer : MonoBehaviour
{

    public TMPro.TextMeshPro text;

    public float ticks;
    public float tickLimit;

    private float startValue;
    public bool grows;
    public float EndSize = 1;

    public float widthOfBullet;

    public float timeOfBullet;

    private float timeend;

    private float endValue;

    public bool diesRightAfter;

    public bool affectZ;

    public bool FadeInText;
    private bool startFadingIn = false;

    public Color EndColor;

    private void Start()
    {
        if (FadeInText)
        {
            text = GetComponent<TMPro.TextMeshPro>();
            EndColor = text.color;
        }

        ticks = Time.time;
        timeend = Time.time + tickLimit;
        endValue = timeend - ticks;
        startValue = ticks;
    }

    private void Update()
    {
        if (ticks > timeend)
        {
            if (diesRightAfter)
            {
                if (GetComponent<DeathTimer>().enabled == false)
                {
                    LeanTween.cancel(this.gameObject);
                }
                GetComponent<DeathTimer>().enabled = true;
            }
        }
        else
        {
            ticks = Time.time;
        }

        float currentSize = EndSize * ((ticks - startValue) / (tickLimit));
        if (grows)
        {
            if (EndSize > 0)
            {
                if (affectZ)
                {
                    gameObject.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                }
                else
                {

                    gameObject.transform.localScale = new Vector3(currentSize, currentSize, 1f);
                }
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-currentSize, currentSize, 1f);
            }
        }
        if (FadeInText)
        {
            if (!startFadingIn)
            {
                startFadingIn = true;
                LeanTween.value(gameObject, updateTextColorCallback, Color.clear, EndColor, (tickLimit)).setEase(LeanTweenType.easeInOutSine);
            }
            /*if(Time.timeScale == 0)
            {
                LeanTween.pause(gameObject);
            }
            else
            {
                if (LeanTween.isPaused(gameObject))
                {
                    LeanTween.resume(gameObject);
                }
            }*/
        }

    }

    void updateTextColorCallback(Color val)
    {
        text.color = val;
    }
}

