using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class WeaponController : damageController
{
    [SerializeField] private GameObject whiteArrow;
    [SerializeField] private SpriteRenderer arrowColor;

    public GameObject pop, pop2;

    private SpriteRenderer sprtrend;
    private float handReachMultiplier;

    public float ReachLength;
    private int DMG;// this is just the running total accumulated while dam_scalign is adding up
    public float DMG_Scaling, DMG_Scaling2;
    public int MaxDamage, MaxDamage2;
    public int MinDamage;
    public float DMGTextSize;

    private Vector3 lastPosition;
    private float totalDistance;

    public Camera camera = null;
    private float timeStamp;
    private float startTime;
    public float activeTimeLimit, activeTimeLimit2;
    public float cooldownTime, cooldownTime2;

    private bool WeaponEnabled, WeaponEnabled2;
    public bool isInHand;

    public Slider StaminaBar;
    private RectTransform rectTrans;

    private GameObject Trail, Trail2;
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

    public float trueMD, movementDelay, movementDelay2;
    private Vector3 _velocity;

    public bool oneSided;
    public bool flipSide;
    private Transform spriteTransform;
    private float xySize;

    delegate void ClickBehavior();
    ClickBehavior clickBehavior;

    public float heldColor = 0f;

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
        OptionsMenu.onMouseVisibilityChangedCallback += changeMouseVisibility;
        changeMouseVisibility();

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
            Destroy(Trail2);
        }
        else // is weapon
        {
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
            isInHand = true;

            Weapon equippedWeapon = EquipmentManager.instance.getMeleeWeapon();
            ReachLength = equippedWeapon.ReachLength;
            ItemReachLength = equippedWeapon.ReachLength;
            DMG_Scaling = equippedWeapon.DMG_Scaling;
            DMG_Scaling2 = equippedWeapon.DMG_Scaling2;
            MaxDamage = equippedWeapon.MaxDamage;
            MaxDamage2 = equippedWeapon.MaxDamage2;
            MinDamage = equippedWeapon.MinDamage;
            DMGTextSize = equippedWeapon.DMGTextSize;
            activeTimeLimit = equippedWeapon.activeTimeLimit;
            activeTimeLimit2 = equippedWeapon.activeTimeLimit2;
            cooldownTime = equippedWeapon.cooldownTime;
            cooldownTime2 = equippedWeapon.cooldownTime2;
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
            movementDelay2 = equippedWeapon.movementDelay2;
            trueMD = movementDelay;

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
            if(Trail2 != null)
            {
                Destroy(Trail2);
            }

            //GetComponentsInChildren<Transform>()[1].localEulerAngles = new Vector3(0,0, equippedWeapon.angle_offset);
            Trail = Instantiate(equippedWeapon.trail, transform);
            Trail.SetActive(false);
            if(equippedWeapon.rightClickStrategy == Equipment.rightClickStrat.DoubleStateCost || equippedWeapon.rightClickStrategy == Equipment.rightClickStrat.DoubleStateDrains)
            {
                Trail2 = Instantiate(equippedWeapon.projectileAttack, transform);
                Trail2.SetActive(false);
            }
            pop = equippedWeapon.popSpawn;
            pop2 = equippedWeapon.popSpawn2;
            DMGText = equippedWeapon.dmgTextObj;
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

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, mouseWorldPosition, ref _velocity, trueMD);
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

        alphaVal = Mathf.SmoothDamp(arrowColor.color.a, heldColor, ref alphaVelo, 0.25f);
        arrowColor.color = new Color(1f, 1f, 1f, alphaVal);

        playerController.UpdateMouseInput();

        zAngle = transform.localEulerAngles.z;

        clickBehavior();


        if (oneSided && playerEntity.bullet == null)
        {
            if (Mathf.Abs(zAngle - lastZAngle) > 0.1f)
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
                if(Trail2 != null)
                    Trail2.transform.localScale = transform.localScale;
            }

        }

        lastZAngle = zAngle;
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
            HasWeakness MG = collision.collider.GetComponent<HasWeakness>();
            float calcDMG = (int)(DMG * multiplier);
            float calcDMGSize = DMGTextSize;


            if (MG == null)
            {
                collision.gameObject.SendMessage("TakeDamage", (int)calcDMG);
            }
            else
            {
                if (MG.WeaknessTo == ElementType || MG.WeaknessTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.WeaknessMultiplier;
                    calcDMGSize = calcDMGSize * MG.WeaknessMultiplier;
                }
                else if (MG.ImmunityTo == ElementType || MG.ImmunityTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.ImmunityMultiplier;
                    calcDMGSize = calcDMGSize * MG.ImmunityMultiplier;
                }

                MG.TakeDamage(calcDMG);
            }

            ShowDMGText((int)calcDMG, calcDMGSize);
            if (WeaponEnabled)
            {
                Instantiate(pop, collision.GetContact(0).point, transform.rotation);
            }
            if (WeaponEnabled2)
            {
                Instantiate(pop2, collision.GetContact(0).point, transform.rotation);
            }
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
            HasWeakness MG = collision.collider.GetComponent<HasWeakness>();
            float calcDMG = (int)(DMG * multiplier);
            float calcDMGSize = DMGTextSize;

            if (MG == null)
            {
                collision.collider.SendMessage("TakeDamage", (int)calcDMG);
            }
            else
            {
                if (MG.WeaknessTo == ElementType || MG.WeaknessTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.WeaknessMultiplier;
                    calcDMGSize = calcDMGSize * MG.WeaknessMultiplier;
                }
                else if (MG.ImmunityTo == ElementType || MG.ImmunityTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.ImmunityMultiplier;
                    calcDMGSize = calcDMGSize * MG.ImmunityMultiplier;
                }
                MG.TakeDamage(calcDMG);
                
            }

            ShowDMGText((int)calcDMG, calcDMGSize);
            if (WeaponEnabled)
            {
                Instantiate(pop, collision.point, transform.rotation);
            }
            if (WeaponEnabled2)
            {
                Instantiate(pop2, collision.point, transform.rotation);
            }
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
    private void enableWeapon2()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
        sprtrend.color = new Vector4(1f, 1f, 1f, 1f);
        startTime = Time.time;
        WeaponEnabled2 = true;
        Trail2.SetActive(true);
    }


    RaycastHit2D[] hits;
    Vector3 currPos;
    public GameObject lastHit;
    float distance;
    Vector2 worldSpaceOffset;
    float zAngle;
    private float lastZAngle;

    float remainingCooldown;

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
        else if (RCS == Equipment.rightClickStrat.DoubleStateDrains)
        {
            clickBehavior += doubleStateRightClicking;
            drainsMana = true;
        }
    }

    private void changeMouseVisibility()
    {
        if (PlayerPrefs.GetInt("mouseVisibility", 0) == 0)
        {
            heldColor = 0f;
        }
        else if (PlayerPrefs.GetInt("mouseVisibility", 0) == 1)
        {
            heldColor = 0.2f;
        }
        else
        {
            heldColor = 0f;
        }
    }
}
