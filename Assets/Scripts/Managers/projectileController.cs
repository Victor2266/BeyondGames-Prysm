using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class projectileController : damageController
{

    public int DMG;

    private Rigidbody2D rb2d;

    public bool Primed = true;
    public bool Bouncy;
    public GameObject pop;
    public GameObject InitialPop = null;
    public float DMGTextSize;

    public float attackVelo;
    public int ManaCost;
    public float coolDownPeriod;
    public bool rapid_fire;
    public bool power_control = false;
    public float inaccuracy = 0;

    public bool DontTouchPlayer;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();


        if (InitialPop != null)
        {
            Vector3 popPosition = new Vector3(transform.position.x, transform.position.y + .05f, InitialPop.transform.position.z);
            Instantiate(InitialPop, popPosition, transform.rotation);
        }
        if (DontTouchPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            CapsuleCollider2D capsuleColider = GetComponent<CapsuleCollider2D>();
            Physics2D.IgnoreCollision(capsuleColider, player.GetComponents<CapsuleCollider2D>()[0], true);
            Physics2D.IgnoreCollision(capsuleColider, player.GetComponents<CapsuleCollider2D>()[1], true);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            BurstIfPrimed();
        }

        if (collision.gameObject.tag == "enemy")
        {
            collision.gameObject.SendMessage("SetCollision", collision.GetContact(0).point);
            collision.gameObject.SendMessage("TakeDamage", DMG);
            BurstIfPrimed();
            ShowDMGText(DMG, DMGTextSize);
        }

        if (collision.gameObject.tag == "boss")
        {
            collision.gameObject.SendMessage("SetCollision", collision.GetContact(0).point);
            collision.gameObject.SendMessage("TakeDamage", DMG / 2f);
            BurstIfPrimed();
            ShowDMGText((DMG / 2), DMGTextSize);
        }
        if (collision.gameObject.tag == "CritBox")
        {

            collision.gameObject.SendMessage("SetCollision", collision.GetContact(0).point);
            collision.gameObject.SendMessage("TakeDamage", DMG * 2);
            BurstIfPrimed();
            ShowDMGText((DMG * 2), DMGTextSize * 2);
        }
        if (collision.gameObject.tag == "EnemyPlayer")
        {
            collision.gameObject.SendMessage("CmdTakeDamage", DMG);
            BurstIfPrimed();
            ShowDMGText(DMG, DMGTextSize);
        }

        if (Bouncy == false && (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Ground"))
        {
            BurstIfPrimed();
        }
    }
    private void BurstIfPrimed()
    {
        if (Primed)
        {
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            GameObject gameObject = Instantiate(pop, transform.position, transform.rotation);
            //NetworkServer.Spawn(gameObject);
            Primed = false;
        }
    }
}