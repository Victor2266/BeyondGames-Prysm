using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour
{
    private Vector2 velocity;

    public float speed;

    public float health;

    public float size;

    public GameObject player;

    public GameObject pop;

    private Rigidbody2D rb2d;

    private Animator anim;

    public bool LookingLeft;

    private bool isDead;

    private float moveHorizontal;

    private GameObject clone;

    public GameObject HealOrb;

    private RaycastHit2D[] hit = new RaycastHit2D[2];

    private Vector3 rayDirection;

    private void Start()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rayDirection = player.transform.position - transform.position;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        
    }

    private void FixedUpdate()
    {
        rayDirection = player.transform.position - transform.position;
        if (!isDead)
        {
            if (LookingLeft)
            {
                transform.localScale = new Vector3(-size, transform.localScale.y, transform.localScale.z);
            }
            else if (!LookingLeft)
            {
                transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
            }
            
        }
        if (isDead)
        {
            StartCoroutine(FreezeInPlace());
        }
    }

    private IEnumerator FreezeInPlace()
    {
        yield return new WaitForSeconds(0.1f);
        if (isDead)
        {
            gameObject.SetActive(false);
            rb2d.velocity = new Vector2(0f, 0f);
        }
        yield break;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDead)
        {
            Physics2D.RaycastNonAlloc(transform.position, rayDirection, hit);
            if (hit[1].collider.name == "Player")
            {
                if (rb2d.position.x > collision.attachedRigidbody.position.x)
                {
                    LookingLeft = true;
                }
                else
                {
                    LookingLeft = false;
                }
                float x = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, Mathf.Abs(player.transform.position.x - transform.position.x) * speed + 0.1f);
                float y = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + Mathf.Abs(transform.position.x - player.transform.position.x), ref velocity.y, Mathf.Abs(transform.position.x - player.transform.position.x) * speed + 0.1f);
                transform.position = new Vector2(x, y);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isDead)
        {
            GetComponent<AudioSource>().Play();
            player.SendMessage("TakeDamage", 20);
            return;
        }
    }

    public void TakeDamage(float amount)
    {
        GetComponent<AudioSource>().Play();
        health -= amount;
        anim.SetTrigger("hurt");
        if (health <= 0f && !isDead)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        clone = Instantiate<GameObject>(HealOrb, transform.position, transform.rotation);
        clone = Instantiate<GameObject>(pop, transform.position, transform.rotation);
    }

    
}