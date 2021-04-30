using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1MiniBossModifiedGoblin : MonoBehaviour
{
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        rb2d = base.GetComponent<Rigidbody2D>();
        anim = base.GetComponent<Animator>();
        StartCoroutine(RandJump(4f));
    }

    private void FixedUpdate()
    {
        Vector2 origin = base.transform.position;
        origin.y -= (size / 2);


        Debug.DrawRay(origin, new Vector3(0f, -0.1f, 0f), Color.red);
        if (aggression == false)
        {
            spear.GetComponent<CircleCollider2D>().enabled = false;
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1f * size, base.transform.localScale.y, base.transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(1f * size, base.transform.localScale.y, base.transform.localScale.z);
            }
        }
        if (aggression == true)
        {
            spear.GetComponent<CircleCollider2D>().enabled = true;
            if (Mathf.Abs(rb2d.velocity.x) > 0)
            {
                anim.SetTrigger("running");
            }
            if (!isDead)
            {
                if (!LookingLeft)
                {
                    base.transform.localScale = new Vector3(-1f * size, base.transform.localScale.y, base.transform.localScale.z);
                }
                else if (LookingLeft)
                {
                    base.transform.localScale = new Vector3(1f * size, base.transform.localScale.y, base.transform.localScale.z);
                }
                if (Mathf.Abs(rb2d.velocity.x) < speed)
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
            if (IsTouchingGround() && isDead)
            {
                base.StartCoroutine(FreezeInPlace());
            }

            if (IsTouchingLeftWall() && TouchingPlayer == false)
            {
                LookingLeft = false;
                StartCoroutine(BackUp(0.1f, 1f * speed));
            }


            else if (IsTouchingRightWall() && TouchingPlayer == false)
            {
                LookingLeft = true;
                StartCoroutine(BackUp(0.1f, -1f * speed));
            }
        }
    }

    private IEnumerator FreezeInPlace()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsTouchingGround() && isDead)
        {
            base.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rb2d.isKinematic = true;
            rb2d.velocity = new Vector2(0f, 0f);
        }
        yield break;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && IsGrounded() && !isDead)
        {
            
            if (TouchingPlayer == false && IsTouchingLeftWall() == false && IsTouchingRightWall() == false)
            {
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


            if (2 == Random.Range(1, 50))
            {
                StartCoroutine(RandJump(4f));
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && IsGrounded() && !isDead)
        {
            anim.SetTrigger("running");
            if (!LookingLeft)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 1f * jumpForce);
            }
            else if (LookingLeft)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 1f * jumpForce);
            }

        }
    }
    private IEnumerator RandJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        rayDirection = player.transform.position - transform.position;
        Physics2D.RaycastNonAlloc(transform.position, rayDirection, hit);
        if (hit[1].collider.name == "Player" && IsGrounded() && !isDead && aggression)
        {
            if (!LookingLeft)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * 1.5f, 1f * jumpForce);
            }
            else if (LookingLeft)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * 1.5f, 1f * jumpForce);
            }
        }
    }
    private IEnumerator BackUp(float delay, float moveHorizontalval)
    {
        yield return new WaitForSeconds(delay);
        moveHorizontal = moveHorizontalval;
        Jump();
    }
    private void Jump()
    {
        if (!isDead)
        {
            if (!LookingLeft)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 1f * jumpForce);
            }
            else if (LookingLeft)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 1f * jumpForce);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isDead)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 10f, rb2d.velocity.y * 2f);
            player.SendMessage("TakeDamage", 20);

            TouchingPlayer = true;
            if (LookingLeft == false)
            {
                moveHorizontal = 1f * speed;
            }
            if (LookingLeft == true)
            {
                moveHorizontal = -1f * speed;
            }
            Jump();
            return;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TouchingPlayer = false;
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = base.transform.position;
        origin.y -= (size);

        return Physics2D.Raycast(origin, -Vector2.up, 0.1f);
    }
    private bool IsTouchingGround()
    {
        Vector2 origin = base.transform.position;
        origin.y -= (1f);

        return Physics2D.Raycast(origin, -Vector2.up, 0.05f);
    }
    private bool IsTouchingLeftWall()
    {
        Vector2 origin = base.transform.position;
        origin.x -= 0.35f;

        Debug.DrawRay(origin, new Vector3(-0.1f, 0f, 0f), Color.red);
        /*Physics2D.RaycastNonAlloc(origin, new Vector3(-0.01f, 0f, 0f), hit2);
        if (hit2[0].collider.name == "Player")
        {
            return false;
        }*/
        return Physics2D.Raycast(origin, Vector2.left, 0.01f);
    }
    private bool IsTouchingRightWall()
    {
        Vector2 origin = base.transform.position;
        origin.x += 0.35f;

        Debug.DrawRay(origin, new Vector3(0.1f, 0f, 0f), Color.red);
        /*Physics2D.RaycastNonAlloc(origin, new Vector3(0.01f, 0f, 0f), hit2);
        if (hit2[0].collider.name == "Player")
        {
            return false;
        }*/
        return Physics2D.Raycast(origin, Vector2.right, 0.01f);
    }
    public void TakeDamage(float amount)
    {
        if (aggression)
        {
            health -= amount;
            anim.SetTrigger("hurt");

            Jump();

            if (health <= 0f && !isDead)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        isDead = true;
        base.gameObject.GetComponentInChildren<Light>().enabled = false;

        clone = Instantiate<GameObject>(HealOrb, new Vector3(0f,0f,0f), base.transform.rotation);
        clone.transform.parent = gameObject.transform;
        clone.transform.localPosition = new Vector3(0,- 0.4f, 0f);
        anim.SetTrigger("dead");
        if (spear != null)
        {
            spear.SetActive(false);
        }
        cape.SetActive(false);
    }
    public float speed;

    public float jumpForce;

    public float health;

    public GameObject player;

    private Rigidbody2D rb2d;

    public GameObject spear = null;

    public GameObject cape;

    private Animator anim;

    public bool LookingLeft;

    public bool isDead;

    private float moveHorizontal;

    private GameObject clone;

    public GameObject HealOrb;

    private RaycastHit2D[] hit = new RaycastHit2D[2];

    private Vector3 rayDirection;

    private bool TouchingPlayer;

    private RaycastHit2D[] hit2 = new RaycastHit2D[2];

    public float size;

    public bool aggression;
}
