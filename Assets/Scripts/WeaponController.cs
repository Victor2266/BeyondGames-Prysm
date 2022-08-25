using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : damageController
{
    [SerializeField]
    private GameObject whiteArrow;
    public GameObject pop;

    private SpriteRenderer sprtrend;

    public float ReachLength;
    private int DMG;
    public float DMG_Scaling;
    public int MaxDamage;
    public float DMGTextSize;


    private Vector3 lastPosition;
    private float totalDistance;

    private float timeStamp;
    private float startTime;
    public float activeTimeLimit;
    public float cooldownTime;

    private bool WeaponEnabled;
    public bool isInHand;

    public Slider StaminaBar;
    private RectTransform rectTrans;

    public GameObject Trail;

    public delegate void OnHeldInHand(bool isHeld);
    public OnHeldInHand onHeldInHand;

    // Start is called before the first frame update
    void Start()
    {
        sprtrend = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        timeStamp = 0f;
        startTime = 0f;
        WeaponEnabled = false;
        rectTrans = StaminaBar.GetComponent<RectTransform>();
        isInHand = false;
        onHeldInHand += HeldInHandStatus;
    }
    void HeldInHandStatus(bool status)
    {
        if (status == false)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0f);
            isInHand = false;
        }
        else
        {
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
            isInHand = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isInHand)
        {
            return;
        }
        //transform.localPosition = whiteArrow.transform.localPosition;
        //transform.localRotation = whiteArrow.transform.localRotation;
        if (transform.localPosition.magnitude > ReachLength)
        {
            transform.localPosition = transform.localPosition.normalized * ReachLength;
        }
        rectTrans.sizeDelta = new Vector2(activeTimeLimit * 100f, 4f);
        StaminaBar.maxValue = activeTimeLimit * 100f;

        if (timeStamp <= Time.time && !WeaponEnabled)
        {
            StaminaBar.value = StaminaBar.maxValue;
        }

        if (Input.GetMouseButtonDown(0) && timeStamp <= Time.time && !WeaponEnabled)
        {
            lastPosition = transform.position;

            enableWeapon();
        }
        if (Input.GetMouseButton(0) && timeStamp <= Time.time && WeaponEnabled)
        {
            float distance = Vector3.Distance(lastPosition, transform.position);
            totalDistance += distance;
            lastPosition = transform.position;
            DMG = (int)(totalDistance * DMG_Scaling);

            StaminaBar.value = StaminaBar.maxValue - ((Time.time - startTime) / activeTimeLimit) * StaminaBar.maxValue;
     
        }
        if ( (Input.GetMouseButtonUp(0) && WeaponEnabled) || (Time.time > startTime + activeTimeLimit && WeaponEnabled) )
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);

            totalDistance = 0f;
            timeStamp = Time.time + cooldownTime;

            StaminaBar.value = 0f;
            WeaponEnabled = false;
            Trail.SetActive(false);
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
            AttackEntity(1f, collision);
        }
        if (collision.gameObject.tag == "boss")
        {
            AttackEntity(0.5f, collision);
        }
        if (collision.gameObject.tag == "CritBox")
        {
            AttackEntity(2f, collision);
        }
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<CapsuleCollider2D>());
        }
    }

    private void AttackEntity(float multiplier, Collision2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", (int) (DMG * multiplier));
        ShowDMGText((int)(DMG * multiplier), DMGTextSize);
        GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        totalDistance = 0f;
    }

    private void enableWeapon()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
        sprtrend.color = new Vector4(1f, 1f, 1f, 1f);
        startTime = Time.time;
        WeaponEnabled = true;
        Trail.SetActive(true);
    }
}
