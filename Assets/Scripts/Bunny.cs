﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MobGeneric
{
    public GameObject squeak;
    // Start is called before the first frame update
    void Start()
    {
        Health = 10f;
        Speed = 1f;
        Height = 0.26f;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        
    }


    // Update is called once per frame
    void Update()
    {   
        if (IsGrounded() && !isDead)
        {   
            if (Random.Range(0, 50.0f) < 0.2f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                rb2d.AddForce(new Vector2(-2f * Time.timeScale, 2f * Time.timeScale), ForceMode2D.Impulse);
                anim.SetTrigger("hop");
            }
            if (Random.Range(0, 50.0f) > 49.8f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                rb2d.AddForce(new Vector2(2f * Time.timeScale, 2f * Time.timeScale), ForceMode2D.Impulse);
                anim.SetTrigger("hop");
            }
            if (Random.Range(0, 50.0f) > 49.9f && Time.timeScale == 1)
            {
                Vector3 Pos = new Vector3(transform.position.x, transform.position.y, -2f);
                GameObject clone;
                clone = Instantiate(squeak, Pos, transform.rotation);
            }
        }
        

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HealItem" || collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<CircleCollider2D>());
            return;
        }
    }
}
