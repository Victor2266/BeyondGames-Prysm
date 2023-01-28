﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImperialKnightLongSword : MobGeneric
{
    [SerializeField]
    private float closeRange, mediumRange, longRange;

    public bool LookingLeft;
    public float jumpForce;
    public Transform player, dash1, dash2, triangle, dash3;
    private float distToPlayer;

    private float lastMode;
    public enemyWeapon enemyWeap;

    public bool thrusting;
    public Rope hair;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.gameObject.GetComponents<CapsuleCollider2D>()[0], true);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.gameObject.GetComponents<CapsuleCollider2D>()[1], true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDead)
        {
            distToPlayer = Vector2.Distance(transform.position, player.position);

            if (distToPlayer > longRange)//LONG RANGE DASHING
            {
                anim.SetTrigger("HangWalk");
                Speed = 1.5f;
                enemyWeap.knockbackX = 50f;
                enemyWeap.knockbackY = 8;
                if (lastMode == mediumRange)
                {
                    if(2 == Random.Range(1, 3))
                        if(IsGrounded())
                            ThrustAttack();
                }

                lastMode = longRange;
            }
            else if (distToPlayer > mediumRange)//chance to shoot proj <<<<<<<<<<<<<<<<<<<<<<<<<
            {
                anim.SetTrigger("HangWalk");
                Speed = 1.5f;
                
                if(lastMode == closeRange)
                {
                    //3 shots forwards
                }
                else if(lastMode == longRange)
                {
                    //4 shot arc
                }
                lastMode = mediumRange;
            }
            else if (distToPlayer > closeRange)//DASH + SWING DOWN on enter and then proj shot random interval <<<<<<<<<<<<<<<<<<<<<<<<<
            {
                Speed = 1f;
                if (!thrusting)
                {
                    anim.SetTrigger("LongpointWalk");
                    enemyWeap.knockbackX = 50f;
                    enemyWeap.knockbackY = 8;

                    //idle walk standard

                    if (lastMode == mediumRange)
                    {
                        DownswingAttack();
                    }
                    lastMode = closeRange;
                }
            }
            else//SWING UP continuously if not currently swining downwards <<<<<<<<<<<<<<<<<<<<<<<<<
            {
                if (!thrusting)
                {
                    anim.SetTrigger("LongpointWalk");
                    enemyWeap.knockbackX = 50f;
                    enemyWeap.knockbackY = 8;

                    if (lastMode == closeRange)
                    {
                        UpswingAttack();
                    }
                    lastMode = 0;
                }
                Speed = 0f;
            }

            if (TouchingPlayer == false && IsTouchingLeftWall() == false && IsTouchingRightWall() == false && !thrusting)
            {
                if (rb2d.position.x > player.position.x)
                {
                    LookingLeft = true;
                    moveHorizontal = -1f * Speed;
                }
                else
                {
                    LookingLeft = false;
                    moveHorizontal = 1f * Speed;
                }
            }
            if (!LookingLeft)
            {
                transform.localScale = new Vector3(-1f * size, size, base.transform.localScale.z);
                dash1.localScale = new Vector3(-2, 1, 2);
                dash2.transform.localScale = new Vector3(-2, 1, 2);
                dash3.transform.localScale = new Vector3(-1, 1, 1);
                triangle.transform.localScale = new Vector3(-1f, 1f, 1f);
                hair.forceGravity = new Vector3(-0.4f,-0.2f,0f);
                healthBar.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (LookingLeft)
            {
                base.transform.localScale = new Vector3(1f * size, size, base.transform.localScale.z);
                dash1.localScale = new Vector3(2, 1, 2);
                dash2.transform.localScale = new Vector3(2, 1, 2);
                dash3.transform.localScale = new Vector3(1, 1, 1);
                triangle.transform.localScale = new Vector3(1f, 1f, 1f);
                hair.forceGravity = new Vector3(0.4f, -0.2f, 0f);
                healthBar.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            if (Mathf.Abs(rb2d.velocity.x) < Speed)
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
            rb2d.gravityScale = 1f;
        }
        if (IsGrounded() && isDead)
        {
            base.StartCoroutine(FreezeInPlace());
        }

        if (IsTouchingLeftWall() && TouchingPlayer == false && !isDead)
        {
            LookingLeft = false;
            thrusting = false;

            rb2d.velocity = (new Vector2(10f, 10f));
            anim.SetTrigger("HangWalk");
            //jump over obstacle
        }
        else if (IsTouchingRightWall() && TouchingPlayer == false && !isDead)
        {
            LookingLeft = true;
            thrusting = false;

            rb2d.velocity = (new Vector2(-10f, 10f));
            anim.SetTrigger("HangWalk");
            //jump over obstacle
        }
        else if (IsTouchingCieling() && !isDead)
        {
            rb2d.gravityScale = 3f;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isDead)
        {
            //PLAY UPWARDS SWING
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 10f * size, 10f);
            player.SendMessage("TakeDamage", 1);


            TouchingPlayer = true;
            Speed = 0f;

            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //swing upwards to block projctiles 50% of time <<<<<<<<<<<<<<<<<<<<<<<<<
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TouchingPlayer = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, closeRange);
        Gizmos.DrawWireSphere(this.transform.position, mediumRange);
        Gizmos.DrawWireSphere(this.transform.position, longRange);
    }

    private void ThrustAttack()
    {
        anim.SetTrigger("Thrusting");
        thrusting = true;
        rb2d.velocity = Vector3.zero;
        if (!LookingLeft)
            rb2d.velocity = (new Vector2(20f, 0f));
        else
            rb2d.velocity = (new Vector2(-20f, 0f));
    }
    private void DownswingAttack()
    {
        anim.SetTrigger("Downswing");
        if (!LookingLeft)
            rb2d.AddForce(new Vector2(140f, 0f), ForceMode2D.Impulse);
        else
            rb2d.AddForce(new Vector2(-140f, 0f), ForceMode2D.Impulse);
    }
    private void UpswingAttack()
    {
        rb2d.velocity = Vector3.zero;
        anim.SetTrigger("Upswing");
    }
}