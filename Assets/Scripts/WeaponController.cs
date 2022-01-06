﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : damageController
{
    [SerializeField]
    private GameObject whiteArrow;
    public GameObject pop;

    private SpriteRenderer sprtrend;

    public float ReachLength;
    private int DMG;
    public float DMG_Scaling;
    public float DMGTextSize;

    private Vector3 lastPosition;
    private float totalDistance;

    private float timeStamp;
    private float startTime;
    public float activeTimeLimit;
    public float cooldownTime;

    private bool WeaponEnabled;

    // Start is called before the first frame update
    void Start()
    {
        sprtrend = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        timeStamp = 0f;
        startTime = 0f;
        WeaponEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = whiteArrow.transform.localPosition;
        //transform.localRotation = whiteArrow.transform.localRotation;
        if (transform.localPosition.magnitude > ReachLength)
        {
            transform.localPosition = transform.localPosition.normalized * ReachLength;
        }

        if (Input.GetMouseButtonDown(0) && timeStamp <= Time.time && !WeaponEnabled)
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
            sprtrend.color = new Vector4(1f, 1f, 1f, 1f);

            lastPosition = transform.position;
            startTime = Time.time;
            WeaponEnabled = true;

        }
        if (Input.GetMouseButton(0) && timeStamp <= Time.time && WeaponEnabled)
        {
            float distance = Vector3.Distance(lastPosition, transform.position);
            totalDistance += distance;
            lastPosition = transform.position;
            DMG = (int)(totalDistance * DMG_Scaling);
        }
        if ( (Input.GetMouseButtonUp(0) && WeaponEnabled) || (Time.time > startTime + activeTimeLimit && WeaponEnabled) )
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);

            totalDistance = 0f;
            timeStamp = Time.time + cooldownTime;

            WeaponEnabled = false;
        }


    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        }
        if (collision.gameObject.tag == "enemy")
        {
            AttackEntity(1f, collision);
        }
        if (collision.gameObject.tag == "boss")
        {
            AttackEntity(0.5f, collision);
        }
        if (collision.gameObject.tag == "CritBox")
        {
            AttackEntity(2f, collision);
        }
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<CapsuleCollider2D>());
        }
    }

    private void AttackEntity(float multiplier, Collision2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", (int) (DMG * multiplier));
        ShowDMGText((int)(DMG * multiplier), DMGTextSize);
        GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        totalDistance = 0f;
    }
}
