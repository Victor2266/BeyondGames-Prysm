using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponController : damageController
{
    [SerializeField] private GameObject whiteArrow;
    [SerializeField] private SpriteRenderer arrowColor;

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
    public PlayerController playerController;
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
    private Equipment.ElementType ElementType;

    public Text DamageCounter;

    AudioSource audioSource;

    public float movementDelay;
    private Vector3 _velocity;

    public bool oneSided;
    public bool flipSide;
    private Transform spriteTransform;
    private float xySize;

    delegate void ClickBehavior();
    ClickBehavior clickBehavior;

    // Start is called before the first frame update
    private void OnEnable()
    {
        onHeldInHand += HeldInHandStatus;
  
        pointerScript = GetComponent<newPointerScript>();
        sprtrend = GetComponentsInChildren<SpriteRenderer>()[1];
        capsuleColider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteTransform = GetComponentsInChildren<Transform>()[1];
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
        if (status == false)// is spell
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0f);
            isInHand = false;

            arrowColor.color = new Color(1f, 1f, 1f, 0.5882353f);
            whiteArrow.SetActive(true);
            OrbPosition.smoothTimeX = 0.05f;
            OrbPosition.smoothTimeY = 0.05f;
            OrbPosition.offsetX = 0f;
            OrbPosition.offsetY = 0.05f;
            DamageCounter.text = "";
            ReachLength = 1.5f;
            Destroy(Trail);
        }
        else // is weapon
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
            ElementType = equippedWeapon.ElementalType;

            SetClickStrat(equippedWeapon.leftClickStrategy, equippedWeapon.rightClickStrategy);

            thrustResetTime = equippedWeapon.thrustResetTime;
            thrustDashDist = equippedWeapon.thrustDashDist;
            thrustShortReach = equippedWeapon.thrustShortReach;//set this equal to the reach length for no recoil when shooting right click

            xySize = equippedWeapon.XYSize;
            transform.localScale = new Vector3(xySize, xySize, 1f);

            pointerScript.offset = 90;
            spriteTransform.localEulerAngles = new Vector3(0, 0, equippedWeapon.angle_offset);

            capsuleColider.offset = equippedWeapon.CapsuleColliderOffset;
            capsuleColider.size = equippedWeapon.CapsuleColliderSize;
            sprtrend.sprite = equippedWeapon.icon;
            handReachMultiplier = equippedWeapon.handReachMultiplier;
            projectileOffset = equippedWeapon.projectileOffset;
            movementDelay = equippedWeapon.movementDelay;

            playerEntity.attack = equippedWeapon.projectileAttack;
            
            arrowColor.color = new Color(0f, 0f, 0f, 0f);
            whiteArrow.SetActive(false);

            OrbPosition.smoothTimeX = 0;
            OrbPosition.smoothTimeY = 0;
            spriteTransform.localPosition = equippedWeapon.SpriteOffset;
            
            audioSource.pitch = equippedWeapon.soundPitch > 0 ? equippedWeapon.soundPitch : 1f;

            if (Trail != null)
            {
                Destroy(Trail);
            }

            //GetComponentsInChildren<Transform>()[1].localEulerAngles = new Vector3(0,0, equippedWeapon.angle_offset);
            Trail = Instantiate(equippedWeapon.trail, transform);
            Trail.SetActive(false);
            pop = equippedWeapon.popSpawn;
            oneSided = equippedWeapon.oneSidedSwing;
            flipSide = equippedWeapon.swingOtherSide;
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
        //Vector2 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        Vector2 mouseWorldPosition = whiteArrow.transform.position - player.transform.position;

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
            if (StaminaBar.value != StaminaBar.maxValue)
            {
                arrowColor.color = new Color(1f, 1f, 1f, 1f);
            }
            StaminaBar.value = StaminaBar.maxValue;
 
        }
        if (ReachLength < ItemReachLength)
        {
            ReachLength = Mathf.SmoothDamp(ReachLength, ItemReachLength, ref itemVelo, thrustResetTime);
        }

        alphaVal = Mathf.SmoothDamp(arrowColor.color.a, 0f, ref alphaVelo, 0.25f);
        arrowColor.color = new Color(1f, 1f, 1f, alphaVal);

        playerController.UpdateMouseInput();

        clickBehavior();

        //defaultLeftClicking();
        //rightClicking();
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
            lastHit = collision.gameObject;

            if (DMG > 0)
            {
                Instantiate(pop, collision.GetContact(0).point, transform.rotation);
            }
            DMG = 0;//resets dmg value for double hits in quick succession 
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
            lastHit = collision.collider.gameObject;
            if(DMG > 0)
            {
                Instantiate(pop, collision.point, transform.rotation);
            }
            DMG = 0;//resets dmg value for double hits in quick succession 
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
        if (DMG > 0)
        {
            collision.gameObject.SendMessage("SetCollision", collision.GetContact(0).point);
            MobGeneric MG = collision.collider.GetComponent<MobGeneric>();
            float calcDMG = (int)(DMG * multiplier);
            float calcDMGSize = DMGTextSize;


            if (MG == null)
            {
                collision.gameObject.SendMessage("TakeDamage", (int)calcDMG);
            }
            else
            {
                if (MG.WeaknessTo == ElementType)
                {
                    calcDMG = calcDMG * MG.WeaknessMultiplier;
                    calcDMGSize = calcDMGSize * MG.WeaknessMultiplier;
                }
                else if (MG.ImmunityTo == ElementType)
                {
                    calcDMG = calcDMG * MG.ImmunityMultiplier;
                    calcDMGSize = calcDMGSize * MG.ImmunityMultiplier;
                }

                MG.TakeDamage(calcDMG);
            }

            ShowDMGText((int)calcDMG, calcDMGSize);
            Instantiate(pop, collision.GetContact(0).point, transform.rotation);
        }
        totalDistance = 0f;//resets dmg calc for next hit
        DMG = 0;//resets dmg value for double hits in quick succession 
    }
    private void AttackEntity(float multiplier, RaycastHit2D collision)
    {
        lastHit = collision.collider.gameObject;

        if(DMG > 0)
        {
            collision.collider.SendMessage("SetCollision", collision.point);
            MobGeneric MG = collision.collider.GetComponent<MobGeneric>();
            float calcDMG = (int)(DMG * multiplier);
            float calcDMGSize = DMGTextSize;

            if (MG == null)
            {
                collision.collider.SendMessage("TakeDamage", (int)calcDMG);
            }
            else
            {
                if (MG.WeaknessTo == ElementType)
                {
                    calcDMG = calcDMG * MG.WeaknessMultiplier;
                    calcDMGSize = calcDMGSize * MG.WeaknessMultiplier;
                }
                else if (MG.ImmunityTo == ElementType)
                {
                    calcDMG = calcDMG * MG.ImmunityMultiplier;
                    calcDMGSize = calcDMGSize * MG.ImmunityMultiplier;
                }
                MG.TakeDamage(calcDMG);
                
            }

            ShowDMGText((int)calcDMG, calcDMGSize);
            Instantiate(pop, collision.point, transform.rotation);
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
    private float lastZAngle;

    private void defaultLeftClicking()//sword strategy/sling weapon/heavysword
    {
        if (PlayerController.startLeft && timeStamp <= Time.time && !WeaponEnabled)//left click that enables weapon
        {
            lastPosition = transform.position;
            lastZAngle = zAngle;
            weaponUI.flashWhite();
            enableWeapon();
            audioSource.Play();
            totalDistance = MinDamage/DMG_Scaling;
            lastHit = null;

            arrowColor.color = new Color(0f, 0f, 0f, 0f);
        }
        if (PlayerController.holdingLeft && timeStamp <= Time.time && WeaponEnabled)//while holding left click
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

            zAngle = transform.localEulerAngles.z;
            if (distance > capsuleColider.size.x * 2f)//capsule checking
            {
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

            if (oneSided)
            {
                if (zAngle - lastZAngle > 0)
                {
                    if (!flipSide)
                        transform.localScale = new Vector3(-xySize, xySize, 1f);
                    else
                        transform.localScale = new Vector3(xySize, xySize, 1f);
                }
                else
                {
                    if (!flipSide)
                        transform.localScale = new Vector3(xySize, xySize, 1f);
                    else
                        transform.localScale = new Vector3(-xySize, xySize, 1f);
                }
            }


            lastPosition = currPos;
            lastZAngle = zAngle;
        }
        if ((PlayerController.liftLeft && WeaponEnabled) || (Time.time > startTime + activeTimeLimit && WeaponEnabled))//released left click
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);

            totalDistance = 0f;

            remainingCooldown = cooldownTime - (1f - (Time.time - startTime) / activeTimeLimit) * cooldownTime;
            timeStamp = Time.time + remainingCooldown;
            weaponUI.ScaleDown(remainingCooldown);
            StaminaBar.value = 0f;
            WeaponEnabled = false;
            Trail.SetActive(false);
            DamageCounter.text = "";

            arrowColor.color = new Color(0f, 0f, 0f, 0f);
            whiteArrow.SetActive(true);
        }
    }
    float remainingCooldown;

    private void defaultRightClicking()
    {
        if (timeStamp <= Time.time)
        {
            if ((PlayerController.startRight || (playerEntity.rapid_fire && PlayerController.holdingRight)) && playerEntity.charges == 1 && playerEntity.currentMana - playerEntity.ManaCost >= 0)
            {
                playerEntity.WeaponUI.ScaleDown(playerEntity.coolDownPeriod);
                weaponUI.flashWhite();
                whiteArrow.SetActive(false);
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
                if (PlayerController.holdingLeft || PlayerController.holdingRight)
                {
                    if (playerEntity.bullet != null)
                    {
                        Vector3 TransNorm = transform.localPosition.normalized;
                        playerEntity.bullet.transform.position = transform.position - TransNorm*projectileOffset;
                        playerEntity.bullet.transform.eulerAngles = transform.localEulerAngles + spriteTransform.localEulerAngles;

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
                else if (PlayerController.liftLeft || PlayerController.liftRight)
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

    private float alphaVelo;
    private float alphaVal;

    private void SetClickStrat(Equipment.leftClickStrat LCS, Equipment.rightClickStrat RCS)
    {
        clickBehavior = null;
        if(LCS == Equipment.leftClickStrat.Default)
        {
            clickBehavior += defaultLeftClicking;
        }

        if(RCS == Equipment.rightClickStrat.Default)
        {
            clickBehavior += defaultRightClicking;
        }
    }
}
