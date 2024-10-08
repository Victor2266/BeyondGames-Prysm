﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGeneric : HasWeakness
{
    public float Health;
    public float MaxHealth;
    public float Speed;
    public float Height, Width, rayLenX;//used to check if grounded

    public bool isDead = false;
    public GameObject DeathItem;
    protected GameObject clone;

    public Animator anim;
    public AudioSource audioSource;
    public Rigidbody2D rb2d;
    public HealthBar healthBar;

    public BloodSplatterer BSplat;

    public float size; //used to find collision
    [SerializeField]
    protected float moveHorizontal;
    protected bool TouchingPlayer;

    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }


    public override void TakeDamage(float amount)
    {
        Health -= amount;
        anim.SetTrigger("hurt");
        healthBar.UpdateHealthBar(Health, MaxHealth);

        BSplat.Spray((int)amount / 3);
        if (Health <= 0f && !isDead)
        {
            Death();
        }
    }


    public virtual void SetCollision(Vector2 pos)
    {
        if(BSplat != null)
            BSplat.SetCollision(pos);
        //this is for hte lbood spray position
    }
    protected virtual void Death()
    {
        isDead = true;
        clone = Instantiate(DeathItem, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), clone.GetComponent<Collider2D>());
        //base.gameObject.GetComponentInChildren<Light>().enabled = false;
        anim.SetTrigger("dead");
        anim.SetBool("FullyDead", true);
        healthBar.gameObject.SetActive(false);
        //Destroy(healthBar.gameObject);
    }
    protected bool IsGrounded()
    {
        Vector2 origin = base.transform.position;
        origin.y -= Height;
        //Debug.DrawRay(origin, new Vector3(0f, -1f, 0f), Color.red);
        return Physics2D.Raycast(origin, -Vector2.up, 0.05f);
    }
    protected bool IsTouchingCieling()
    {
        Vector2 origin = base.transform.position;
        origin.y += Height;

        return Physics2D.Raycast(origin, Vector2.up, 0.05f);
    }
    protected bool IsTouchingLeftWall()
    {
        Vector2 origin = base.transform.position;
        origin.x -= Width;

        //Debug.DrawRay(origin, new Vector3(-rayLenX, 0f, 0f), Color.red);
        /*Physics2D.RaycastNonAlloc(origin, new Vector3(-0.01f, 0f, 0f), hit2);
        if (hit2[0].collider.name == "Player")
        {
            return false;
        }*/
        return Physics2D.Raycast(origin, Vector2.left, rayLenX * size);
    }
    protected bool IsTouchingRightWall()
    {
        Vector2 origin = base.transform.position;
        origin.x += Width;

        //Debug.DrawRay(origin, new Vector3(rayLenX, 0f, 0f), Color.red);
        /*Physics2D.RaycastNonAlloc(origin, new Vector3(0.01f, 0f, 0f), hit2);
        if (hit2[0].collider.name == "Player")
        {
            return false;
        }*/
        return Physics2D.Raycast(origin, Vector2.right, rayLenX * size);
    }
    protected IEnumerator FreezeInPlace()
    {
        yield return new WaitForSeconds(0.5f);
        if (IsGrounded() && isDead)
        {
            base.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rb2d.isKinematic = true;
            rb2d.velocity = new Vector2(0f, 0f);
        }
        yield break;
    }

    public GameObject TextObject;
    public float textYOffset;
    private GameObject nextMsg;
    private GameObject thisMsg = null;
    public Transform SpeakingArea;
    public virtual void ShowText(float waitForAmount, string txt, float size)
    {
        if(TextObject == null)
        {
            return;
        }
        if (nextMsg != null)
        {
            thisMsg = nextMsg;
        }
        nextMsg = Instantiate(TextObject);
        InGameTextMessage IGTM = nextMsg.GetComponent<InGameTextMessage>();
        if (SpeakingArea == null)
            IGTM.followThis = transform;
        else
            IGTM.followThis = SpeakingArea;
        IGTM.textYOffset = textYOffset;
        IGTM.lastMSG = thisMsg;

        if (thisMsg != null)
        {
            IGTM.moveLastMSG();
            //lastMsg.transform.localPosition = new Vector3(0f, 0.4f, 0f);
            //lastMsg.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 1.4f, 0f);
        }
        nextMsg.transform.localPosition = new Vector3(0f, 0.2f, 0f);

        TMPro.TextMeshPro NM_TMPRO = nextMsg.GetComponent<TMPro.TextMeshPro>();
        NM_TMPRO.text = txt;
        NM_TMPRO.fontSize = size;
        nextMsg.GetComponent<DeathTimer>().tickLimit = waitForAmount * 1.1f;
        //NetworkServer.Spawn(gameObject2);
    }
}
