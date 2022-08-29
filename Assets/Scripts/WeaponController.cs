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
    private float handReachMultiplier;

    public float ReachLength;
    private int DMG;// this is just the running total accumulated while dam_scalign is adding up
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

    public OldCameraController OrbPosition;
    private PointerScript pointerScript;
    private CapsuleCollider2D capsuleColider;
    public WeaponUI weaponUI;
    public PlayerEntity playerEntity;
    public float projectileOffset;
    private float ItemReachLength;
    private float itemVelo;

    private float thrustResetTime;
    private float thrustDashDist;
    private float thrustShortReach;//set this equal to the reach length for no recoil when shooting right click
    // Start is called before the first frame update
    void Start()
    {
        pointerScript = GetComponent<PointerScript>();
        sprtrend = GetComponent<SpriteRenderer>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
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

            whiteArrow.SetActive(true);
            OrbPosition.smoothTimeX = 0.05f;
            OrbPosition.smoothTimeX = 0.05f;
        }
        else
        {
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
            isInHand = true;

            Weapon equippedWeapon = EquipmentManager.instance.getMeleeWeapon();
            ReachLength = equippedWeapon.ReachLength;
            ItemReachLength = equippedWeapon.ReachLength;
            DMG_Scaling = equippedWeapon.DMG_Scaling;
            MaxDamage = equippedWeapon.MaxDamage;
            DMGTextSize = equippedWeapon.DMGTextSize;
            activeTimeLimit = equippedWeapon.activeTimeLimit;
            cooldownTime = equippedWeapon.cooldownTime;

            thrustResetTime = equippedWeapon.thrustResetTime;
            thrustDashDist = equippedWeapon.thrustDashDist;
            thrustShortReach = equippedWeapon.thrustShortReach;//set this equal to the reach length for no recoil when shooting right click

            transform.localScale = new Vector3(equippedWeapon.XYSize, equippedWeapon.XYSize, 1f);
            pointerScript.offset = equippedWeapon.angle_offset;

            capsuleColider.offset = equippedWeapon.CapsuleColliderOffset;
            capsuleColider.size = equippedWeapon.CapsuleColliderSize;
            sprtrend.sprite = equippedWeapon.icon;
            handReachMultiplier = equippedWeapon.handReachMultiplier;
            projectileOffset = equippedWeapon.projectileOffset;

            playerEntity.attack = equippedWeapon.projectileAttack;
            
            whiteArrow.SetActive(false);
            OrbPosition.smoothTimeX = 0.01f;
            OrbPosition.smoothTimeX = 0.01f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
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
        OrbPosition.offsetX = transform.localPosition.x * handReachMultiplier;
        OrbPosition.offsetY = transform.localPosition.y * handReachMultiplier;
        rectTrans.sizeDelta = new Vector2(activeTimeLimit * 100f, 4f);
        StaminaBar.maxValue = activeTimeLimit * 100f;

        if (timeStamp <= Time.time && !WeaponEnabled)
        {
            StaminaBar.value = StaminaBar.maxValue;
        }
        if (ReachLength < ItemReachLength)
        {
            ReachLength = Mathf.SmoothDamp(ReachLength, ItemReachLength, ref itemVelo, thrustResetTime);
        }

        leftClicking();
        rightClicking();


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

    private void leftClicking()
    {
        if (Input.GetMouseButtonDown(0) && timeStamp <= Time.time && !WeaponEnabled)//left click that enables weapon
        {
            lastPosition = transform.position;
            weaponUI.flashWhite();
            enableWeapon();
        }
        if (Input.GetMouseButton(0) && timeStamp <= Time.time && WeaponEnabled)//while holding left click
        {
            float distance = Vector3.Distance(lastPosition, transform.position);
            totalDistance += distance;
            lastPosition = transform.position;
            DMG = (int)(totalDistance * DMG_Scaling);
            StaminaBar.value = StaminaBar.maxValue - ((Time.time - startTime) / activeTimeLimit) * StaminaBar.maxValue;
        }
        if ((Input.GetMouseButtonUp(0) && WeaponEnabled) || (Time.time > startTime + activeTimeLimit && WeaponEnabled))//released left click
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);

            totalDistance = 0f;
            timeStamp = Time.time + cooldownTime - (1f - (Time.time - startTime) / activeTimeLimit)*cooldownTime;
            weaponUI.ScaleDown(cooldownTime - (1f - (Time.time - startTime) / activeTimeLimit) * cooldownTime);
            StaminaBar.value = 0f;
            WeaponEnabled = false;
            Trail.SetActive(false);
        }
    } 
    private void rightClicking()
    {
        shootSpecialAttack();
    }
    private void shootSpecialAttack()
    {
        if (timeStamp <= Time.time)
        {
            if ((Input.GetMouseButtonDown(1) || (playerEntity.rapid_fire && Input.GetMouseButton(1))) && playerEntity.charges == 1 && playerEntity.currentMana - playerEntity.ManaCost >= 0)
            {
                playerEntity.WeaponUI.ScaleDown(playerEntity.coolDownPeriod);
                weaponUI.flashWhite();
                sprtrend.color = new Vector4(1f, 1f, 1f, 1f);
                StaminaBar.value = 0f;
                playerEntity.bullet = Instantiate(playerEntity.attack, transform.position, transform.rotation);
                playerEntity.currentMana -= playerEntity.ManaCost;
                playerEntity.charges = 0;
                ReachLength = thrustShortReach;
                Dash();
                if (playerEntity.weapon < 8)
                {
                    playerEntity.bullet.GetComponent<projectileController>().Primed = false;
                }
            }
            if (playerEntity.charges == 0)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    if (playerEntity.bullet != null)
                    {
                        Vector3 TransNorm = transform.localPosition.normalized;
                        playerEntity.bullet.transform.position = transform.position - TransNorm*projectileOffset;
                        playerEntity.bullet.transform.eulerAngles = transform.localEulerAngles;

                        if (playerEntity.power_control == false && playerEntity.inaccuracy == 0)
                        {
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerEntity.attackVelo * TransNorm.x, playerEntity.attackVelo * TransNorm.y);
                        }
                        else if (playerEntity.power_control && playerEntity.inaccuracy == 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, playerEntity.CameraOrbObj.transform.position);
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerEntity.attackVelo * TransNorm.x * scaled_pow, playerEntity.attackVelo * TransNorm.y * scaled_pow);
                        }
                        else if (playerEntity.power_control == false && playerEntity.inaccuracy > 0)
                        {
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(playerEntity.attackVelo * TransNorm.x - playerEntity.inaccuracy, playerEntity.attackVelo * TransNorm.x + playerEntity.inaccuracy),
                                Random.Range(playerEntity.attackVelo * TransNorm.y - playerEntity.inaccuracy, playerEntity.attackVelo * TransNorm.y + playerEntity.inaccuracy));
                        }
                        if (playerEntity.power_control && playerEntity.inaccuracy > 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, playerEntity.CameraOrbObj.transform.position);
                            float velo = playerEntity.attackVelo * 2 * scaled_pow;
                            playerEntity.bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(velo * TransNorm.x - playerEntity.inaccuracy, velo * TransNorm.x + playerEntity.inaccuracy),
                                Random.Range(velo * TransNorm.y - playerEntity.inaccuracy, velo * TransNorm.y + playerEntity.inaccuracy));
                        }
                    }

                }
                if (playerEntity.rapid_fire == true)
                {
                    timeStamp = Time.time + playerEntity.coolDownPeriod;
                    playerEntity.charges = 1;
                    if (playerEntity.bullet != null)
                    {
                        playerEntity.bullet.GetComponent<projectileController>().Primed = true;
                    }

                }
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    timeStamp = Time.time + playerEntity.coolDownPeriod;
                    playerEntity.charges = 1;
                    if (playerEntity.bullet != null)
                    {
                        playerEntity.bullet.GetComponent<projectileController>().Primed = true;
                    }


                }
                sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
            }
        }
    }
    private void Dash()
    {
        //GameObject dash = Instantiate(playerEntity.speedTrail, transform);
        //dash.transform.position = transform.position;
        //playerEntity.speedTrail.SetActive(true);

        //playerEntity.Flinch = true;
        playerEntity.rb2d.velocity = new Vector2(thrustDashDist * transform.localPosition.x, 0.6f * thrustDashDist * transform.localPosition.y);
        //dash.transform.eulerAngles = transform.eulerAngles;
        
    }
}
