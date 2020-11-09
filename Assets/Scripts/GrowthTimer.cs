using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthTimer : MonoBehaviour
{
    private void Start()
    {
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
                GetComponent<DeathTimer>().enabled = true;
            }
        }
        else
        {
            ticks = Time.time;
        }

        if (grows)
        {
            if (EndSize > 0)
            {
                gameObject.transform.localScale = new Vector3(EndSize * ((ticks - startValue) / (tickLimit)), EndSize * ((ticks - startValue) / (tickLimit)), 1f);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-EndSize * ((ticks - startValue) / (tickLimit)), EndSize * ((ticks - startValue) / (tickLimit)), 1f);
            }
        }
    }

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
    
}

