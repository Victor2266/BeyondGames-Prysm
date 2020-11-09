using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
    public Sprite redBOSS;
    public GameObject explosion;

    private bool ChangedToRed = false;

    private void Start()
    {
        rb2d = base.GetComponent<Rigidbody2D>();
        healthObj = GetComponent<HealthBarHealth>();
    }

    private void FixedUpdate()
    {
        if (rb2d.position.x > 46f)
        {
            velo = new Vector2(-speed, velo.y);
        }
        else if (rb2d.position.x < 32f)
        {
            velo = new Vector2(speed, velo.y);
        }
        if (rb2d.position.y > -25.5f)
        {
            velo = new Vector2(velo.x, -speed);
        }
        else if (rb2d.position.y < -35.6f)
        {
            velo = new Vector2(velo.x, speed);
        }
        rb2d.velocity = new Vector2(velo.x, velo.y);
        if (healthObj.health <= 0f && !isDead)
        {
            Death();
        }
        else if (healthObj.health <= 60f)
        {
            speed = 6f;
            GetComponent<SpriteRenderer>().sprite = redBOSS;
            if (ChangedToRed == false)
            {
                GetComponent<AudioSource>().Play();
                ChangedToRed = true;
            }
        }
        else if (healthObj.health <= 120f)
        {
            speed = 4f;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 10f, rb2d.velocity.y * 4f);
            player.SendMessage("TakeDamage", 20);
        }
        if (collision.gameObject.tag == "platform")
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision, true);
        }
    }

    public void TakeDamage(float amount)
    {
        mobTimer++;
        if (mobTimer > 10)
        {
            mobTimer -= 10;
            if (healthObj.health < 60f)
            {
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            else if (healthObj.health < 120f)
            {
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
        }
        manaTimer++;
        if (manaTimer > 5)
        {
            manaTimer -= 5;
            clone = UnityEngine.Object.Instantiate<GameObject>(ManaDrops, base.transform.position, base.transform.rotation);
        }
        healthObj.health -= amount;
    }

    private void Death()
    {
        isDead = true;
        Instantiate<GameObject>(explosion, base.transform.position, base.transform.rotation);
        Instantiate<GameObject>(explosion, base.transform.position, base.transform.rotation);
        Instantiate<GameObject>(explosion, base.transform.position, base.transform.rotation);
        base.gameObject.SetActive(false);
        Instantiate<GameObject>(ExtraNeon, base.transform.position, base.transform.rotation);
        BossRoomRange.GetComponent<AudioSource>().enabled = false;
        defeatBossMsg.SetActive(false);
        ChargeEnabler.SetActive(true);
    }

    public Vector2 velo = new Vector2(2f, 2f);

    public GameObject BossRoomRange;
    public GameObject player;
    public GameObject defeatBossMsg;
    public GameObject ManaDrops;

    public GameObject HealthDrops;

    public GameObject MobDrop;
    public HealthBarHealth healthObj;

    public GameObject ExtraNeon;

    public float speed = 2f;

    private GameObject clone;

    private int mobTimer;

    private int manaTimer;

    private Rigidbody2D rb2d;

    private bool isDead;

    public GameObject ChargeEnabler;
}