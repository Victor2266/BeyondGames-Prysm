using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImperialKnightLongSword : MobGeneric
{
    [SerializeField]
    private float closeRange, mediumRange, longRange, longestRange;

    public bool LookingLeft;
    public float jumpForce;
    public Transform player, dash1, dash2, triangle, dash3, heldWeapon;
    public float distToPlayer;

    private float lastMode;
    public enemyWeapon enemyWeap;

    public bool thrusting, upswinging;
    public Rope hair;

    public GameObject projAttack;
    private int hits;

    public GameObject DeathItem2;
    public AudioClip blockedSound, speakSound;

    private CapsuleCollider2D thisColider;

    public bool agression = false;

    private bool puppetMode;

    // Start is called before the first frame update
    void Start()
    {
        thisColider = GetComponent<CapsuleCollider2D>();
        Physics2D.IgnoreCollision(thisColider, player.gameObject.GetComponents<CapsuleCollider2D>()[0], true);
        Physics2D.IgnoreCollision(thisColider, player.gameObject.GetComponents<CapsuleCollider2D>()[1], true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!agression)
        {
            distToPlayer = Vector2.Distance(transform.position, player.position);
            return;
        }
        if (!isDead)
        {
            distToPlayer = Vector2.Distance(transform.position, player.position);

            if (!puppetMode)
            {

                if (distToPlayer > longestRange)
                {
                    anim.SetTrigger("HangWalk");
                    Speed = 3.5f;
                    enemyWeap.knockbackX = 50f;
                    enemyWeap.knockbackY = 8;
                    if (lastMode == longRange && transform.position.y < player.position.y - 1f)
                    {
                        ShowText(2f, "GET BACK HERE", 1f);
                        ThrustAttack2();
                    }

                    lastMode = longestRange;
                }
                else if (distToPlayer > longRange)//LONG RANGE DASHING
                {
                    anim.SetTrigger("HangWalk");
                    Speed = 1.5f;
                    enemyWeap.knockbackX = 50f;
                    enemyWeap.knockbackY = 8;
                    if (lastMode == mediumRange)
                    {
                        ShowText(2f, "enguard", 1f);
                        if (2 == Random.Range(1, 3))
                            if (IsGrounded())
                                ThrustAttack();
                    }

                    lastMode = longRange;
                }
                else if (distToPlayer > mediumRange)//chance to shoot proj <<<<<<<<<<<<<<<<<<<<<<<<<
                {
                    anim.SetTrigger("HangWalk");
                    Speed = 1.5f;

                    if (2 == Random.Range(1, 4))
                    {
                        if (lastMode == closeRange)
                        {
                            //3 shots forwards
                            ThreeStrike();
                        }
                        else if (lastMode == longRange)
                        {
                            //4 shot arc
                            FourStrike();
                        }
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
                            ShowText(2f, "DIE.", 1f);
                            DownswingAttack();
                        }
                        else
                        {
                            if (hits >= 3)
                            {
                                JumpAway();
                            }
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
                            ShowText(2f, "begone", 1f);
                            UpswingAttack();
                        }
                        else if (lastMode == 0)
                        {
                            if (!upswinging)
                            {
                                if (distToPlayer < 1.5f)
                                {
                                    JumpAway();
                                }

                            }
                        }
                        else
                        {
                            if (hits >= 3)
                            {
                                JumpAway();
                            }
                        }
                        lastMode = 0;
                    }
                    Speed = 0.5f;
                }
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

            rb2d.velocity = (new Vector2(10f, Random.RandomRange(2, 10)));
            hits = 0;
            anim.SetTrigger("HangWalk");
            
        }
        else if (IsTouchingRightWall() && TouchingPlayer == false && !isDead)
        {
            LookingLeft = true;
            thrusting = false;

            rb2d.velocity = (new Vector2(-10f, Random.RandomRange(2, 10)));
            hits = 0;
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
        enemyWeap.DMG = 20;
        anim.SetTrigger("Thrusting");
        thrusting = true;
        rb2d.velocity = Vector3.zero;
        if (!LookingLeft)
            rb2d.velocity = (new Vector2(20f, 0f));
        else
            rb2d.velocity = (new Vector2(-20f, 0f));
    }
    private void ThrustAttack2()
    {
        enemyWeap.DMG = 20;
        anim.SetTrigger("Thrusting");
        thrusting = true;
        rb2d.velocity = Vector3.zero;
        if (!LookingLeft)
            rb2d.velocity = (new Vector2(20f, 10f));
        else
            rb2d.velocity = (new Vector2(-20f, 10f));
    }
    public void DownswingAttack()
    {
        enemyWeap.DMG = 20;
        anim.SetTrigger("Downswing");
        if (!LookingLeft)
            rb2d.AddForce(new Vector2(140f, 0f), ForceMode2D.Impulse);
        else
            rb2d.AddForce(new Vector2(-140f, 0f), ForceMode2D.Impulse);
    }
    private void UpswingAttack()
    {
        enemyWeap.DMG = 10f;
        rb2d.velocity = Vector3.zero;
        anim.SetTrigger("Upswing");
    }
    private void ThreeStrike()
    {
        enemyWeap.DMG = 10f;
        anim.SetTrigger("ThreeStrike");
    }
    public float speedOfProj;
    private void ThreeStrikeProjection()
    {
        clone = Instantiate(projAttack, heldWeapon.position, heldWeapon.rotation);
        float SpearAngle = heldWeapon.eulerAngles.z -90f;
        Vector3 v = new Vector3(speedOfProj*Mathf.Cos(Mathf.Deg2Rad * (SpearAngle)), speedOfProj*Mathf.Sin(Mathf.Deg2Rad * (SpearAngle)), 0f);
        clone.GetComponent<Rigidbody2D>().velocity = v;

        Physics2D.IgnoreCollision(thisColider, clone.GetComponent<CapsuleCollider2D>(), true);
    }
    private void FourStrike()
    {
        enemyWeap.DMG = 10f;
        anim.SetTrigger("FourStrike");
    }

    public override void TakeDamage(float amount)
    {
        if (!thrusting)
            Health -= amount;
        else
        {
            ShowText(0.5f, "<color=red>BLOCKED", 2f);
            audioSource.PlayOneShot(blockedSound, 1f);
        }
        hits++;
        if(amount > 25f)
        {
            hits++;
        }
        healthBar.UpdateHealthBar(Health, MaxHealth);

        BSplat.Spray((int)amount / 3);
        if (Health <= 0f && !isDead)
        {
            Death();
            return;
        }
    }
    protected override void Death()
    {
        isDead = true;
        Instantiate(DeathItem, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
        Instantiate(DeathItem2, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
        //base.gameObject.GetComponentInChildren<Light>().enabled = false;
        anim.SetTrigger("dead");
        anim.SetBool("FullyDead", true);
        healthBar.gameObject.SetActive(false);
        //Destroy(healthBar.gameObject);
    }
    private void JumpAway()
    {
        if (LookingLeft)
        {
            rb2d.velocity = (new Vector2(10f, Random.RandomRange(2,10)));
        }
        else
        {
            rb2d.velocity = (new Vector2(-10f, Random.RandomRange(2, 10)));
        }
        hits = 0;
    }

    public void ActivatePuppetMode()
    {
        puppetMode = true;
        Health = 500f;
        isDead = false;
        anim.SetTrigger("idle2");
        base.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        rb2d.isKinematic = false;
        healthBar.gameObject.SetActive(true);

    }
}
