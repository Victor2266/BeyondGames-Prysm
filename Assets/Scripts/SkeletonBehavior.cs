using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonBehavior : MonoBehaviour
{
    private void Start()
    {
        healthScript = GetComponent<HealthBarHealth>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!LookingLeft)
            {
                transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
            }
            else if (LookingLeft)
            {
                transform.localScale = new Vector3(-size, transform.localScale.y, transform.localScale.z);
            }

            if (Mathf.Abs(rb2d.velocity.x) <= speed && Mathf.Abs(rb2d.velocity.y) <= speed)
            {
                rb2d.velocity = new Vector2(moveHorizontal, rb2d.velocity.y);
            }
        }
        if (IsGrounded())
        {
            if (moveHorizontal > 0f)
            {
                if (!isDead)
                {
                    moveHorizontal -= 0.1f;
                }
            }
            else if (moveHorizontal < 0f && !isDead)
            {
                moveHorizontal += 0.1f;
            }
        }
        if (IsGrounded() && isDead)
        {
            StartCoroutine(FreezeInPlace());
        }
    }

    private IEnumerator FreezeInPlace()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsGrounded() && isDead)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rb2d.isKinematic = true;
            rb2d.velocity = new Vector2(0f, 0f);
        }
        yield break;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && IsGrounded() && !isDead)
        {
            anim.SetBool("walking", true);
            if (rb2d.position.x > collision.attachedRigidbody.position.x)
            {
                LookingLeft = true;
                moveHorizontal = -1f * speed;
            }
            else
            {
                LookingLeft = false;
                moveHorizontal = 1f * speed;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("walking", false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isDead)
        {
            StartCoroutine(Delay(collision));
            player.SendMessage("TakeDamage", 20);
            anim.SetTrigger("attack");
            return;
        }
    }

    private IEnumerator Delay(Collision2D collision)
    {
        collision.gameObject.SendMessage("Flinching");
        yield return new WaitForSeconds(0.2f);
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 20f, 1f);
        yield break;
    }

    private bool IsGrounded()
    {

        Vector2 origin = transform.position;
        origin.y -= 1.6f;
        return Physics2D.Raycast(origin, -Vector2.up, 0.005f);
    }

    public void TakeDamage(float amount)
    {
        healthScript.health-= amount;
        anim.SetTrigger("hurt");
        rb2d.velocity = Vector2.up * 3f;
        if (healthScript.health<= 0f && !isDead)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        clone = UnityEngine.Object.Instantiate<GameObject>(HealOrb, transform.position, transform.rotation);
        anim.SetBool("alive", false);
        anim.SetTrigger("dead");
    }

    public float speed;

    public float size;

    private HealthBarHealth healthScript;

    public GameObject player;

    private Rigidbody2D rb2d;

    private Animator anim;

    public bool LookingLeft;

    private bool isDead;

    private float moveHorizontal;

    private GameObject clone;

    public GameObject HealOrb;
}