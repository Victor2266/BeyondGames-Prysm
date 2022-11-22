using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MobGeneric
{
    public GameObject squeak;
    private SpriteRenderer sprd;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 1f;
        Height = 0.26f;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprd = GetComponent<SpriteRenderer>();
        
    }


    // Update is called once per frame
    void Update()
    {   
        if (IsGrounded() && !isDead)
        {   
            if (Random.Range(0, 50.0f) < 0.2f)
            {
                //transform.localScale = new Vector3(1f, 1f, 1f);
                sprd.flipX = false;
                rb2d.AddForce(new Vector2(-2f * Time.timeScale, 2f * Time.timeScale), ForceMode2D.Impulse);
                anim.SetTrigger("hop");
            }
            if (Random.Range(0, 50.0f) > 49.8f)
            {
                //transform.localScale = new Vector3(-1f, 1f, 1f);
                sprd.flipX = true;
                rb2d.AddForce(new Vector2(2f * Time.timeScale, 2f * Time.timeScale), ForceMode2D.Impulse);
                anim.SetTrigger("hop");
            }
            if (Random.Range(0, 50.0f) > 49.9f && Time.timeScale == 1)
            {
                Vector3 Pos = new Vector3(transform.position.x, transform.position.y, -2f);
                GameObject clone;
                clone = Instantiate(squeak, transform);
            }
        }
        

    }
    public void TakeDamage(float amount)
    {
        Health -= amount;
        anim.SetTrigger("hurt");

        if (Health <= 0f && !isDead)
        {
            Death();
        }
    }

    private GameObject clone;
    private void Death()
    {
        isDead = true;
        clone = Instantiate(DeathItem, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), clone.GetComponent<Collider2D>());
        anim.SetTrigger("hurt");
        anim.SetBool("alive", false);
        GetComponent<CircleCollider2D>().radius = 0.08f;
        gameObject.tag = "box";
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
