using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGeneric : MonoBehaviour
{
    public float Health;
    public float Speed;
    public float Height;
    public bool isDead = false;
    public GameObject DeathItem;
    private GameObject clone;

    public Animator anim;
    public Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGrounded()
    {
        Vector2 origin = base.transform.position;
        origin.y -= Height;
        return Physics2D.Raycast(origin, -Vector2.up, 0.005f);
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

    private void Death()
    {
        isDead = true;
        clone = Instantiate<GameObject>(DeathItem, transform.position, transform.rotation);
        anim.SetBool("alive", false);
        anim.SetTrigger("death");
    }

}
