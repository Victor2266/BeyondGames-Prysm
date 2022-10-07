using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratBehavior : MonoBehaviour
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
        origin.y -= (size/2);


        //Debug.DrawRay(origin, new Vector3(0f, -0.1f, 0f), Color.red);

        if (!isDead)
        {
            if (!LookingLeft)
            {
                base.transform.localScale = new Vector3(-1f * size, size, base.transform.localScale.z);
                healthBar.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (LookingLeft)
            {
                base.transform.localScale = new Vector3(1f * size, size, base.transform.localScale.z);
                healthBar.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            if ( Mathf.Abs(rb2d.velocity.x) < speed)
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
            anim.SetTrigger("running");
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


    GameObject exclaimation = null;
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


            //spawn exclaimation point
            if (exclaimation == null)
            {
                exclaimation = Instantiate(exclaimMark, transform);
                exclaimation.transform.localPosition = new Vector3(0f, 0.68f, 0f);
            }
        }

        
    }
    private IEnumerator RandJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        rayDirection = player.transform.position - transform.position;
        Physics2D.RaycastNonAlloc(transform.position, rayDirection, hit);
        if (hit[1].collider.name == "Player" && IsGrounded() && !isDead)
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
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 10f * size, rb2d.velocity.y * 2f);
            player.SendMessage("TakeDamage", 10);

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
            anim.SetTrigger("attacking");

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
        origin.y -= (size/2);

        return Physics2D.Raycast(origin, -Vector2.up, 0.05f);
    }
    private bool IsTouchingLeftWall()
    {
        Vector2 origin = base.transform.position;
        origin.x -= 0.35f *size;

        //Debug.DrawRay(origin, new Vector3(-0.1f, 0f, 0f), Color.red);
        /*Physics2D.RaycastNonAlloc(origin, new Vector3(-0.01f, 0f, 0f), hit2);
        if (hit2[0].collider.name == "Player")
        {
            return false;
        }*/
            return Physics2D.Raycast(origin, Vector2.left, 0.01f * size);
    }
    private bool IsTouchingRightWall()
    {
        Vector2 origin = base.transform.position;
        origin.x += 0.35f *size;

        //Debug.DrawRay(origin, new Vector3(0.1f, 0f, 0f), Color.red);
        /*Physics2D.RaycastNonAlloc(origin, new Vector3(0.01f, 0f, 0f), hit2);
        if (hit2[0].collider.name == "Player")
        {
            return false;
        }*/
        return Physics2D.Raycast(origin, Vector2.right, 0.01f * size);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        anim.SetTrigger("hurt");
        healthBar.UpdateHealthBar(health, 50f);
        Jump();
        Instantiate(bloodSplatter, collisionPosition, Quaternion.identity);
        bloodSplatter.GetComponent<ParticleSystem>().Emit((int)amount);
        if (health <= 0f && !isDead)
        {
            Death();
        }
    }
    private Vector3 collisionPosition;
    public void SetCollision(Vector2 pos)
    {
        collisionPosition = pos;
        bloodDir = transform.position - new Vector3(pos.x, pos.x, 0f);
        ZAngle = Mathf.Atan2(bloodDir.y, bloodDir.x) * Mathf.Rad2Deg + bloodOffset;
        _lookRot = Quaternion.AngleAxis(ZAngle, Vector3.forward);
        transform.rotation = _lookRot;
        
    }
    private Vector3 bloodDir;
    public float bloodOffset;
    private float ZAngle;
    private Quaternion _lookRot;

    private void Death()
    {
        isDead = true;
        clone = Instantiate(HealOrb, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), clone.GetComponent<Collider2D>());
        base.gameObject.GetComponentInChildren<Light>().enabled = false;
        anim.SetTrigger("dead");
        anim.SetBool("FullyDead", true);
        if (spear != null)
        {
            spear.SetActive(false);
        }
        healthBar.gameObject.SetActive(false);
        //Destroy(healthBar.gameObject);
    }

    public HealthBar healthBar;

    public float speed;

    public float jumpForce;

    public float health;

    public GameObject player;

    private Rigidbody2D rb2d;

    public GameObject spear = null;

    private Animator anim;

    public bool LookingLeft;

    public bool isDead;

    private float moveHorizontal;

    private GameObject clone;

    public GameObject bloodSplatter;

    public GameObject HealOrb;

    private RaycastHit2D[] hit = new RaycastHit2D[2];

    private Vector3 rayDirection;

    private bool TouchingPlayer;

    private RaycastHit2D[] hit2 = new RaycastHit2D[2];

    public float size;

    public GameObject exclaimMark;
}