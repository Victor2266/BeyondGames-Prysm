using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    private void Start()
    {
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
                gameObject.transform.localScale = new Vector3(startSize * (timeend - ticks) / endValue, startSize * (timeend - ticks) / endValue, 1f);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-startSize * (timeend - ticks) / endValue, startSize * (timeend - ticks) / endValue, 1f);
            }
            if (trail)
            {
                trailRenderer.widthMultiplier = (timeend - ticks) / endValue * widthOfBullet;
                trailRenderer.time = (timeend - ticks) / endValue * timeOfBullet;
            }
        }
        if (glowsBright && ticks > timeend - lightObject.GetComponent<ArtificialLightFlicker>().duration / 2)
        {
            lightObject.SetActive(true);
        }
    }

    public float ticks;
    public bool glowsBright;
    public GameObject lightObject;

    public float tickLimit;

    public bool shrinks;

    public bool trail;
    public float startSize = 1;
    public float widthOfBullet;

    public float timeOfBullet;

    private float timeend;

    private float endValue;

    public bool explodes;
    public GameObject explosionPrefab;

    private TrailRenderer trailRenderer;
}
