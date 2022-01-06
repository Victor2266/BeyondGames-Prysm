using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : damageController
{
    [SerializeField]
    private GameObject whiteArrow;
    public GameObject pop;

    private SpriteRenderer sprtrend;

    public float ReachLength;
    public int DMG;
    public float DMGTextSize;
    // Start is called before the first frame update
    void Start()
    {
        sprtrend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = whiteArrow.transform.localPosition;
        //transform.localRotation = whiteArrow.transform.localRotation;

        if (transform.localPosition.magnitude > ReachLength)
        {
            transform.localPosition = transform.localPosition.normalized * ReachLength;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
            sprtrend.color = new Vector4(1f, 1f, 1f, 1f);

        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "box")
        {
            GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        }

        if (collision.gameObject.tag == "enemy")
        {
            collision.gameObject.SendMessage("TakeDamage", DMG);
            ShowDMGText(DMG, DMGTextSize);
            GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        }

        if (collision.gameObject.tag == "boss")
        {
            collision.gameObject.SendMessage("TakeDamage", DMG / 2f);
            ShowDMGText((DMG / 2), DMGTextSize);
        }
        if (collision.gameObject.tag == "CritBox")
        {
            collision.gameObject.SendMessage("TakeDamage", DMG * 2);
            ShowDMGText((DMG * 2), DMGTextSize * 2);
        }
        if (collision.gameObject.tag == "EnemyPlayer")
        {
            collision.gameObject.SendMessage("CmdTakeDamage", DMG);
            ShowDMGText(DMG, DMGTextSize);
        }
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<CapsuleCollider2D>());
        }
    }
}
