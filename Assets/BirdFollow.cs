using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFollow : MonoBehaviour
{
    private Vector2 velocity;

    public float speed;

    public float size;

    public GameObject player;

    public GameObject pop;

    private Rigidbody2D rb2d;

    private Animator anim;

    public bool LookingLeft;

    private bool isDead;

    private float moveHorizontal;

    private GameObject clone;

    public GameObject HealOrb;

    private RaycastHit2D[] hit = new RaycastHit2D[2];

    private Vector3 rayDirection;

    private void Start()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rayDirection = player.transform.position - transform.position;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();


    }

    private void FixedUpdate()
    {
        rayDirection = player.transform.position - transform.position;
        if (!isDead)
        {
            if (LookingLeft)
            {
                transform.localScale = new Vector3(-size, transform.localScale.y, transform.localScale.z);
            }
            else if (!LookingLeft)
            {
                transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDead)
        {
            Physics2D.RaycastNonAlloc(transform.position, rayDirection, hit);
            if (hit[1].collider.name == "Player")
            {
                float x;
                if (rb2d.position.x > collision.attachedRigidbody.position.x)
                {
                    LookingLeft = true;
                    x = Mathf.SmoothDamp(transform.position.x, player.transform.position.x + 1f, ref velocity.x, speed + 0.1f);
                }
                else
                {
                    LookingLeft = false;
                    x = Mathf.SmoothDamp(transform.position.x, player.transform.position.x - 1f, ref velocity.x, speed + 0.1f);
                }

                float y = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + Mathf.Abs(transform.position.x - player.transform.position.x), ref velocity.y, speed + 0.1f);
                transform.position = new Vector2(x, y);
            }
        }
    }

}
