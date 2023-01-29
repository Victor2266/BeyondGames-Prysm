using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGeneric : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float Speed;
    public float Height, Width, rayLenX;//used to check if grounded

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
    public float size; //used to find collision
    [SerializeField]
    protected float moveHorizontal;
    protected bool TouchingPlayer;

    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
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
}
