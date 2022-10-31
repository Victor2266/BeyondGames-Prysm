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
    public int MinDamage;
    public float DMGTextSize;

    private Vector3 lastPosition;
    private float totalDistance;

    public Camera camera = null;
    private float timeStamp;
    private float startTime;
    public float activeTimeLimit;
    public float cooldownTime;

    private bool WeaponEnabled;
    public bool isInHand;

    public Slider StaminaBar;
    private RectTransform rectTrans;

    private GameObject Trail;
    public GameObject player;
    public delegate void OnHeldInHand(bool isHeld);
    public OnHeldInHand onHeldInHand;

    public OldCameraController OrbPosition;
    private newPointerScript pointerScript;
    private CapsuleCollider2D capsuleColider;
    public WeaponUI weaponUI;
    public PlayerEntity playerEntity;
    public float projectileOffset;
    private float ItemReachLength;
    private float itemVelo;

    private float thrustResetTime;
    private float thrustDashDist;
    private float thrustShortReach;//set this equal to the reach length for no recoil when shooting right click
    private bool projAsChild;

    public Text DamageCounter;

    AudioSource audioSource;

    public float movementDelay;
    private Vector3 _velocity;

    // Start is called before the first frame update
    private void OnEnable()
    {
        onHeldInHand += HeldInHandStatus;
  
        pointerScript = GetComponent<newPointerScript>();
        sprtrend = GetComponent<SpriteRenderer>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
        timeStamp = 0f;
        startTime = 0f;
        WeaponEnabled = false;
        rectTrans = StaminaBar.GetComponent<RectTransform>();
        isInHand = false;

        Physics2D.IgnoreCollision(capsuleColider, player.GetComponents<CapsuleCollider2D>()[0], true);
        Physics2D.IgnoreCollision(capsuleColider, player.GetComponents<CapsuleCollider2D>()[1], true);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

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
            OrbPosition.smoothTimeY = 0.05f;
            OrbPosition.offsetX = 0f;
            OrbPosition.offsetY = 0.05f;
            DamageCounter.text = "";
            Destroy(Trail);
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
            MinDamage = equippedWeapon.MinDamage;
            DMGTextSize = equippedWeapon.DMGTextSize;
            activeTimeLimit = equippedWeapon.activeTimeLimit;
            cooldownTime = equippedWeapon.cooldownTime;
            projAsChild = equippedWeapon.projAsChild;

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
            movementDelay = equippedWeapon.movementDelay;

            playerEntity.attack = equippedWeapon.projectileAttack;
            
            whiteArrow.SetActive(false);
            OrbPosition.smoothTimeX = 0.01f;
            OrbPosition.smoothTimeY = 0.01f;
            
            audioSource.pitch = equippedWeapon.soundPitch > 0 ? equippedWeapon.soundPitch : 1f;

            if (Trail != null)
            {
                Destroy(Trail);
            }
            Trail = Instantiate(equippedWeapon.trail, transform);
            Trail.SetActive(false);
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
        Vector2 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, mouseWorldPosition, ref _velocity, movementDelay);
        //transform.localPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0f);

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
        if(collision.gameObject == lastHit )//will not hit the same obj as the capsule
        {
            return;
        }

        //if weapon directly contacts obj then the capsule will not hit it again
        if (collision.gameObject.tag == "box" || collision.gameObject.tag == "enemyProj")
        {
            GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
            lastHit = collision.gameObject;
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
    public void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("exiting " + collision.collider.tag);
        if (collision.collider.tag == "box" || collision.collider.tag == "enemyProj")
        {
            lastHit = null;//reset so you can hit twice after swinging for a bit
            currPos = transform.position;
        }
        if (collision.collider.tag == "enemy")
        {
            lastHit = null;//reset so you can hit twice after swinging for a bit
            currPos = transform.position;
        }
        if (collision.collider.tag == "boss")
        {
            lastHit = null;//reset so you can hit twice after swinging for a bit
            currPos = transform.position;
        }
        if (collision.collider.tag == "CritBox")
        {
            lastHit = null;//reset so you can hit twice after swinging for a bit
            currPos = transform.position;
        }
    }
    public void DoubleCheckingCollision(RaycastHit2D collision)
    {
        if (collision.collider.tag == "box" || collision.collider.tag == "enemyProj")//if capsule alone hits obj then the weapon can't hit it with the colider again unless weapon is outside the colider first
        {
            GameObject gameObject = Instantiate(pop, collision.point, transform.rotation);
            lastHit = collision.collider.gameObject;
        }
        if (collision.collider.tag == "enemy")
        {
            AttackEntity(1f, collision);
        }
        if (collision.collider.tag == "boss")
        {
            AttackEntity(0.5f, collision);
        }
        if (collision.collider.tag == "CritBox")
        {
            AttackEntity(2f, collision);
        }
    }

    private void AttackEntity(float multiplier, Collision2D collision)
    {
        lastHit = collision.gameObject;

        collision.gameObject.SendMessage("SetCollision", collision.GetContact(0).point);
        collision.gameObject.SendMessage("TakeDamage", (int) (DMG * multiplier));
        if (DMG > 0)
        {
            ShowDMGText((int)(DMG * multiplier), DMGTextSize);
            GameObject gameObject = Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        }
        totalDistance = 0f;
        DMG = 0;
    }
    private void AttackEntity(float multiplier, RaycastHit2D collision)
    {
        lastHit = collision.collider.gameObject;

        collision.collider.SendMessage("SetCollision", collision.point);
        collision.collider.SendMessage("TakeDamage", (int)(DMG * multiplier));
        if(DMG > 0)
        {
            ShowDMGText((int)(DMG * multiplier), DMGTextSize);
            GameObject gameObject = Instantiate(pop, collision.point, transform.rotation);
        }
        totalDistance = 0f;
        DMG = 0;
    }

    private void enableWeapon()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
        sprtrend.color = new Vector4(1f, 1f, 1f, 1f);
        startTime = Time.time;
        WeaponEnabled = true;
        Trail.SetActive(true);
    }

    RaycastHit2D[] hits;
    Vector3 currPos;
    public GameObject lastHit;
    float distance;
    Vector2 worldSpaceOffset;
    float zAngle;
    private void leftClicking()//sword strategy/sling weapon/heavysword
    {
        if (Input.GetMouseButtonDown(0) && timeStamp <= Time.time && !WeaponEnabled)//left click that enables weapon
        {
            lastPosition = transform.position;
            weaponUI.flashWhite();
            enableWeapon();
            audioSource.Play();
            totalDistance = MinDamage/DMG_Scaling;
            lastHit = null;
        }
        if (Input.GetMouseButton(0) && timeStamp <= Time.time && WeaponEnabled)//while holding left click
        {
            currPos = transform.position;
            distance = Vector3.Distance(lastPosition, currPos);//USED IN DAMAGE CALC

            if(distance > ReachLength * 1.3f && DMG > MaxDamage * 0.1)
            {
                audioSource.Play();
            }

            totalDistance += distance;
            DMG = (int)(totalDistance * DMG_Scaling);
            if (DMG > MaxDamage)
            {
                DMG = MaxDamage;
            }
            DamageCounter.text = DMG.ToString();

            if(MaxDamage > 0)
                DamageCounter.color = Color.Lerp(Color.yellow, Color.red,(float) DMG/MaxDamage);
            else
                DamageCounter.color = Color.Lerp(Color.yellow, Color.red, (float)DMG / 100f);

            StaminaBar.value = StaminaBar.maxValue - ((Time.time - startTime) / activeTimeLimit) * StaminaBar.maxValue;

            if (distance > capsuleColider.size.x * 2f)//capsule checking
            {
                zAngle = transform.localEulerAngles.z;
                worldSpaceOffset = new Vector2(-Mathf.Sin(zAngle * Mathf.Deg2Rad) * capsuleColider.offset.y, Mathf.Cos(zAngle * Mathf.Deg2Rad) * capsuleColider.offset.y);//this is the y offset, no xoffset yet
                hits = Physics2D.CapsuleCastAll((Vector2)currPos + worldSpaceOffset, capsuleColider.size, CapsuleDirection2D.Vertical, transform.localEulerAngles.z, lastPosition - currPos, distance);//used to check if passing through hitboxes
                //Debug.DrawRay((Vector2)currPos + worldSpaceOffset, lastPosition - currPos, Color.red, 10.0f);

                foreach (RaycastHit2D hit in hits)
                {

                    if (capsuleColider.IsTouching(hit.collider))//if weapon is in colider, no need for casting
                    {
                        lastHit = hit.collider.gameObject;
                    }
                    if (lastHit == null)
                    {
                        DoubleCheckingCollision(hit);
                    }
                    else if (lastHit != (hit.collider.gameObject))
                    {
                        //Debug.Log(lastHit.name);
                        DoubleCheckingCollision(hit);
                    }

                }


            }

            lastPosition = currPos;
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
            DamageCounter.text = "";
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
                if (projAsChild == true)
                {
                    playerEntity.bullet = Instantiate(playerEntity.attack, transform.position, transform.rotation, transform);
                }
                else
                {
                    playerEntity.bullet = Instantiate(playerEntity.attack, transform.position, transform.rotation);
                }
                playerEntity.setMana(playerEntity.currentMana - playerEntity.ManaCost);
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

        playerEntity.rb2d.velocity = new Vector2(thrustDashDist * transform.localPosition.normalized.x, 0.6f * thrustDashDist * transform.localPosition.normalized.y);
        //playerEntity.rb2d.AddForce(new Vector2(thrustDashDist * transform.localPosition.x, 0.6f * thrustDashDist * transform.localPosition.y), ForceMode2D.Impulse);
        
        /*
        if (transform.localPosition.x > 0)
        {
            playerEntity.rb2d.AddForce(new Vector2(16f, 0f), ForceMode2D.Impulse);
        }
        else
        {
            playerEntity.rb2d.AddForce(new Vector2(-16f, 0f), ForceMode2D.Impulse);
        }*/

        //dash.transform.eulerAngles = transform.eulerAngles;

    }
}
