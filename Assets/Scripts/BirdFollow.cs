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

    public float distance;

    public Transform redEye;

    private void Start()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rayDirection = player.transform.position - transform.position;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        hurtSayings = new string[]{ "OUCH", "STOP THAT", "that HURT"};
    }

    private void FixedUpdate()
    {
        rayDirection = player.transform.position - transform.position;
        if (!isDead)
        {
            if (LookingLeft)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                redEye.localPosition = new Vector3(-0.153f, -0.01f, -0.1f);

            }
            else if (!LookingLeft)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                redEye.localPosition = new Vector3(0.153f, -0.01f, -0.1f);
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
                    x = Mathf.SmoothDamp(transform.position.x, player.transform.position.x + distance, ref velocity.x, speed + 0.1f);
                }
                else
                {
                    LookingLeft = false;
                    x = Mathf.SmoothDamp(transform.position.x, player.transform.position.x - distance, ref velocity.x, speed + 0.1f);
                }

                float y = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + Mathf.Abs(transform.position.x - player.transform.position.x), ref velocity.y, speed + 0.1f);
                transform.position = new Vector2(x, y);
            }
        }
    }
    public void TakeDamage(float amount)
    {
        GetComponent<AudioSource>().Play();
        anim.SetTrigger("hurt");
        //show ouch text
        ShowDMGText();
    }

    public GameObject DMGText;
    public string[] hurtSayings= {"OUCH", "STOP THAT", "that HURT"};
    public void ShowDMGText()
    {
        GameObject gameObject2 = Instantiate(DMGText, transform.position, transform.rotation);
        int randint = Random.Range(0, hurtSayings.Length);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = hurtSayings[randint];
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-50.0f, 50.0f));

        //NetworkServer.Spawn(gameObject2);
    }
}
