using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class projectileController : MonoBehaviour
{

    public int DMG;

    private Rigidbody2D rb2d;

    public bool Primed = true;
    public bool Bouncy;
    public GameObject pop;
    public GameObject InitialPop = null;
    public GameObject DMGText;
    public float DMGTextSize;

    public float attackVelo;
    public int ManaCost;
    public float coolDownPeriod;
    public bool rapid_fire;
    public bool power_control = false;
    public float inaccuracy = 0;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();


        if (InitialPop != null)
        {
            Vector3 popPosition = new Vector3(transform.position.x, transform.position.y + .05f, InitialPop.transform.position.z);
            Instantiate(InitialPop, popPosition, transform.rotation);
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
            collision.gameObject.SendMessage("TakeDamage", DMG);
            BurstIfPrimed();
            ShowDMGText(DMG, DMGTextSize);
        }

        if (collision.gameObject.tag == "boss")
        {
            collision.gameObject.SendMessage("TakeDamage", DMG / 2f);
            BurstIfPrimed();
            ShowDMGText((DMG / 2), DMGTextSize);
        }
        if (collision.gameObject.tag == "CritBox")
        {
            collision.gameObject.SendMessage("TakeDamage", DMG * 2);
            BurstIfPrimed();
            ShowDMGText((DMG * 2), DMGTextSize * 2);
        }
        if(collision.gameObject.tag == "EnemyPlayer")
        {
            collision.gameObject.SendMessage("CmdTakeDamage", DMG);
            BurstIfPrimed();
            ShowDMGText(DMG, DMGTextSize);
        }
        
        if (Bouncy == false && collision.gameObject.tag == "Untagged")
        {
            BurstIfPrimed();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
        }
    }
    private void BurstIfPrimed()
    {
        if (Primed)
        {
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            GameObject gameObject = Instantiate(pop, transform.position, transform.rotation);
            NetworkServer.Spawn(gameObject);
            Primed = false;
        }
    }
    private void ShowDMGText(int damage, float size)
    {
        GameObject gameObject2 = Instantiate(DMGText, transform.position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = damage.ToString();
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-50.0f, 50.0f));


        NetworkServer.Spawn(gameObject2);
    }
}