using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpecialWeaponDoor : MonoBehaviour
{
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb2d.position.y > TopPos.y)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.position = new Vector2(TopPos.x, TopPos.y);
            gameObject.GetComponent<StationaryShaking>().enabled = false;
        }
        if (rb2d.position.y < BotPos.y)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.position = new Vector2(BotPos.x, BotPos.y);
            gameObject.GetComponent<StationaryShaking>().enabled = false;
        }
        if (rb2d.position.x < TopPos.x)//left most
        {
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
            rb2d.position = new Vector2(TopPos.x, TopPos.y);
        }
        if (rb2d.position.x > BotPos.x)//right most
        {
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
            rb2d.position = new Vector2(BotPos.x, BotPos.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == SpecificWeapon.name + "(Clone)")
        {
            GlowingJem.SetActive(true);
            if(transform.position.x == TopPos.x)
            {
                gameObject.GetComponent<StationaryShaking>().enabled = false;
                GoRight();
            }
            else if (transform.position.x == BotPos.x)
            {
                gameObject.GetComponent<StationaryShaking>().enabled = false;
                GoLeft();
            }
            if (TopPos.y == BotPos.y)
            {

            }
            else if (transform.position.y == TopPos.y)
            {
                gameObject.GetComponent<StationaryShaking>().enabled = true;
                GoBot();
            }
            else if (transform.position.y == BotPos.y)
            {
                gameObject.GetComponent<StationaryShaking>().enabled = true;
                GoTop();
            }
        }
    }

    public void GoBot()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, -speed);
    }

    public void GoTop()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, speed);
    }
    public void GoLeft()
    {
        rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
    }
    public void GoRight()
    {
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
    }

    public Vector2 TopPos;

    public Vector2 BotPos;

    public float speed;

    public GameObject SpecificWeapon;

    private Rigidbody2D rb2d;
    public GameObject GlowingJem;
}