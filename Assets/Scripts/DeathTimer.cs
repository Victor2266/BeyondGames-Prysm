using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathTimer : MonoBehaviour
{
    private TMPro.TextMeshPro text;

    public float ticks;
    public bool glowsBright;
    public GameObject lightObject;

    public float tickLimit;

    public bool shrinks;
    public bool TMPFades;

    public bool trail;
    public float startSize = 1;
    public float widthOfBullet;

    public float timeOfBullet;

    private float timeend;

    private float endValue;

    public bool explodes;
    public GameObject explosionPrefab;

    private TrailRenderer trailRenderer;
    private Vector3 _velo;
    private bool startFading;
    private void Start()
    {
        if (TMPFades)
        {
            text = GetComponent<TMPro.TextMeshPro>();
        }
        if (trail)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }
        ticks = Time.time;
        timeend = Time.time + tickLimit;
        endValue = timeend - ticks;
    }

    private void Update()
    {
        if (ticks > timeend)
        {
            //gameObject.SetActive(false);
            LeanTween.cancel(this.gameObject);
            Destroy(this.gameObject);
            if (explodes)
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
            }
        }
        else
        {
            ticks = Time.time;
        }
        if (shrinks)
        {
            if (gameObject.transform.localScale.x > 0)
            {
                //gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, Vector3.zero, ref _velo, tickLimit/3f);
                gameObject.transform.localScale = new Vector3(startSize * (timeend - ticks) / endValue, startSize * (timeend - ticks) / endValue, 1f);
            }
            else
            {
                //gameObject.transform.localScale = Vector3.SmoothDamp(gameObject.transform.localScale, Vector3.zero, ref _velo, tickLimit/3f);
                gameObject.transform.localScale = new Vector3(-startSize * (timeend - ticks) / endValue, startSize * (timeend - ticks) / endValue, 1f);
            }
            if (trail)
            {
                trailRenderer.widthMultiplier = (timeend - ticks) / endValue * widthOfBullet;
                trailRenderer.time = (timeend - ticks) / endValue * timeOfBullet;
            }
        }
        if (TMPFades)
        {
            if(((timeend - ticks) / endValue) < 0.25f && !startFading)
            {
                startFading = true;
                LeanTween.value(gameObject, updateTextColorCallback, text.color, Color.clear, ((timeend - ticks) / endValue)).setEase(LeanTweenType.easeInOutSine);
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
        if (glowsBright && ticks > timeend - lightObject.GetComponent<ArtificialLightFlicker>().duration / 2)
        {
            lightObject.SetActive(true);
        }
    }


    void updateTextColorCallback(Color val)
    {
        text.color = val;
    }
}
