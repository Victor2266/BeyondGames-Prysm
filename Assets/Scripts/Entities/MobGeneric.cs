using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGeneric : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float Speed;
    public float Height;//used to check if grounded
    public bool isDead = false;
    public GameObject DeathItem;
    protected GameObject clone;

    public Animator anim;
    public Rigidbody2D rb2d;
    public HealthBar healthBar;

    public BloodSplatterer BSplat;

    public Equipment.ElementType WeaknessTo;
    public float WeaknessMultiplier = 1;
    public Equipment.ElementType ImmunityTo;
    public float ImmunityMultiplier = 1;

    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    public bool IsGrounded()
    {
        Vector2 origin = base.transform.position;
        origin.y -= Height;
        return Physics2D.Raycast(origin, -Vector2.up, 0.005f);
    }

    public virtual void TakeDamage(float amount)
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


    public virtual void SetCollision()
    {
        //this is for hte lbood spray position
    }
    protected void Death()
    {
        isDead = true;
        clone = Instantiate(DeathItem, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), clone.GetComponent<Collider2D>());
        base.gameObject.GetComponentInChildren<Light>().enabled = false;
        anim.SetTrigger("dead");
        anim.SetBool("FullyDead", true);
        healthBar.gameObject.SetActive(false);
        //Destroy(healthBar.gameObject);
    }
}
