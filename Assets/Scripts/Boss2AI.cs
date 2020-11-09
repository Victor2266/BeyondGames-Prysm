using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI : MonoBehaviour
{
    private void Start()
    {
        transform.eulerAngles = pos;
        rb2d = GetComponent<Rigidbody2D>();
        isDead = false;
        HealBarHealth = GetComponent<HealthBarHealth>();
        HealBarHealth.health = 500f;
        SwipePhase = true;
        DeathScript = GetComponent<DeathTimer>();
    }

    private void FixedUpdate()
    {
        if (HealBarHealth.health < 100f)
        {
            speed = 8f;
        }
        else if (HealBarHealth.health < 250f)
        {
            speed = 4f;
        }
        else if (HealBarHealth.health > 500f)
        {
            speed = 2f;
        }
        if (transform.position.x < -24f || transform.position.x > 18.75f)
        {
            SwipePhase = true;
        }
        if (SwipePhase)
        {
            Swipe();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDead && !SwipePhase)
        {
            if (rb2d.position.x > collision.attachedRigidbody.position.x)
            {
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            if (Mathf.Abs(rb2d.position.x - collision.attachedRigidbody.position.x) < 0.1f)
            {
                rb2d.velocity = new Vector2(0f, -speed);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isDead)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 10f, rb2d.velocity.y * 2f);
            collision.gameObject.SendMessage("TakeDamage", 20);
            if (SwipePhase)
            {
                SwipePhase = false;
            }
            else if (!SwipePhase)
            {
                SwipePhase = true;
            }
            return;
        }
    }

    private void Swipe()
    {
        if (transform.position.x < -24f)
        {
            LookingLeft = false;
        }
        else if (transform.position.x > 18.75f)
        {
            LookingLeft = true;
        }
        if (LookingLeft)
        {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
        }
        else if (!LookingLeft)
        {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }
    }

    public void TakeDamage(float amount)
    {
        HealBarHealth.health -= amount;
        if (SwipePhase)
        {
            SwipePhase = false;
        }
        else if (!SwipePhase)
        {
            SwipePhase = true;
        }
        if (HealBarHealth.health <= 0f && !isDead)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        clone = UnityEngine.Object.Instantiate<GameObject>(HealOrb, transform.position, transform.rotation);
        DeathScript.enabled = true;
        defeatBossMSG.SetActive(false);
    }

    private Vector3 pos = Vector3.zero;

    public float speed;

    private Rigidbody2D rb2d;
    
    private bool LookingLeft;

    private DeathTimer DeathScript;
    private bool isDead;

    public bool SwipePhase;

    private GameObject clone;

    public GameObject HealOrb;
    private HealthBarHealth HealBarHealth;
    public GameObject defeatBossMSG;
}