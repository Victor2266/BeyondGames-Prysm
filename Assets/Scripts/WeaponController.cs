using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class WeaponController : damageController
{
    [SerializeField] private GameObject whiteArrow;
    [SerializeField] private SpriteRenderer arrowColor;

    public GameObject pop2;
    private SpriteRenderer sprtrend;
    private Vector3 lastPosition;
    private float DMG;
    private float totalDistance;

    public Camera camera = null;
    private float timeStamp;
    private float startTime;

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

    private Equipment.ElementType activeElement;

    public Text DamageCounter;

    AudioSource audioSource;

    public float trueMD;
    private Vector3 _velocity;

    private Transform spriteTransform;

    delegate void ClickBehavior();
    ClickBehavior clickBehavior;

    private bool showWeapIndicator;

    public float heldColor = 0f;
    public float ReachLength;

    Weapon equippedWeapon;
    DoubleStateWeapon equippedDoubleStateWeapon;
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

        OptionsMenu.onWeapVisibilityChangedCallback += changeWeapVisibility;
        changeWeapVisibility();

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
            ReachLength = 1.5f;// delete if unused later
            Destroy(Trail);
            Destroy(Trail2);
        }
        else // is weapon
        {
            sprtrend.color = new Vector4(1f, 1f, 1f, 0.5f);
            isInHand = true;

            equippedWeapon = EquipmentManager.instance.getMeleeWeapon();

            if(equippedWeapon is DoubleStateWeapon)
            {
                equippedDoubleStateWeapon = (DoubleStateWeapon)equippedWeapon;
                pop2 = equippedDoubleStateWeapon.popSpawn2;

                if (Trail2 != null)
                {
                    Destroy(Trail2);
                }
                Trail2 = Instantiate(equippedWeapon.projectileAttack, transform);
                Trail2.SetActive(false);
            }
            ReachLength = equippedWeapon.ReachLength;


            SetClickStrat(equippedWeapon.leftClickStrategy, equippedWeapon.rightClickStrategy);

            transform.localScale = new Vector3(equippedWeapon.XYSize, equippedWeapon.XYSize, 1f);

            pointerScript.offset = 90;
            spriteTransform.localEulerAngles = new Vector3(0, 0, equippedWeapon.angle_offset);

            capsuleColider.offset = equippedWeapon.CapsuleColliderOffset;
            capsuleColider.size = equippedWeapon.CapsuleColliderSize;
            sprtrend.sprite = equippedWeapon.icon;
            trueMD = equippedWeapon.movementDelay;

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

            DMGText = equippedWeapon.dmgTextObj;
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
        OrbPosition.offsetX = transform.localPosition.x * equippedWeapon.handReachMultiplier;
        OrbPosition.offsetY = transform.localPosition.y * equippedWeapon.handReachMultiplier;
        rectTrans.sizeDelta = new Vector2(equippedWeapon.activeTimeLimit * 100f, 4f);
        StaminaBar.maxValue = equippedWeapon.activeTimeLimit * 100f;

        if (timeStamp <= Time.time && !WeaponEnabled && !WeaponEnabled2)
        {
            if (StaminaBar.value != StaminaBar.maxValue)
            {
                arrowColor.color = new Color(1f, 1f, 1f, 1f);
                //show text ready to attack again when option is selected
                if(showWeapIndicator)
                    ShowText("ready", 2, Color.white);
            }
            StaminaBar.value = StaminaBar.maxValue;
 
        }
        if (ReachLength < equippedWeapon.ReachLength)
        {
            ReachLength = Mathf.SmoothDamp(ReachLength, equippedWeapon.ReachLength, ref itemVelo, equippedWeapon.thrustResetTime);
        }

        alphaVal = Mathf.SmoothDamp(arrowColor.color.a, heldColor, ref alphaVelo, 0.25f);
        arrowColor.color = new Color(1f, 1f, 1f, alphaVal);

        playerController.UpdateMouseInput();

        zAngle = transform.localEulerAngles.z;

        clickBehavior();


        if (equippedWeapon.oneSidedSwing && playerEntity.bullet == null)
        {
            if (Mathf.Abs(zAngle - lastZAngle) > 0.1f)
            {
                if (zAngle - lastZAngle > 0)
                {
                    if (!equippedWeapon.swingOtherSide)
                        transform.localScale = new Vector3(-equippedWeapon.XYSize, equippedWeapon.XYSize, 1f);
                    else
                        transform.localScale = new Vector3(equippedWeapon.XYSize, equippedWeapon.XYSize, 1f);
                }
                else
                {
                    if (!equippedWeapon.swingOtherSide)
                        transform.localScale = new Vector3(equippedWeapon.XYSize, equippedWeapon.XYSize, 1f);
                    else
                        transform.localScale = new Vector3(-equippedWeapon.XYSize, equippedWeapon.XYSize, 1f);
                }
                //if(Trail2 != null)
                    //Trail2.transform.localScale = transform.localScale;
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
                Instantiate(equippedWeapon.popSpawn, collision.GetContact(0).point, transform.rotation);
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
                Instantiate(equippedWeapon.popSpawn, collision.point, transform.rotation);
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
            float calcDMGSize = equippedWeapon.DMGTextSize;


            if (MG == null)
            {
                collision.gameObject.SendMessage("TakeDamage", (int)calcDMG);
            }
            else
            {
                if (MG.WeaknessTo == activeElement || MG.WeaknessTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.WeaknessMultiplier;
                    calcDMGSize = calcDMGSize * MG.WeaknessMultiplier;
                }
                else if (MG.ImmunityTo == activeElement || MG.ImmunityTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.ImmunityMultiplier;
                    calcDMGSize = calcDMGSize * MG.ImmunityMultiplier;
                }

                MG.TakeDamage(calcDMG);
            }

            ShowDMGText((int)calcDMG, calcDMGSize);
            if (WeaponEnabled)
            {
                Instantiate(equippedWeapon.popSpawn, collision.GetContact(0).point, transform.rotation);
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
            float calcDMGSize = equippedWeapon.DMGTextSize;

            if (MG == null)
            {
                collision.collider.SendMessage("TakeDamage", (int)calcDMG);
            }
            else
            {
                if (MG.WeaknessTo == activeElement || MG.WeaknessTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.WeaknessMultiplier;
                    calcDMGSize = calcDMGSize * MG.WeaknessMultiplier;
                }
                else if (MG.ImmunityTo == activeElement || MG.ImmunityTo == Equipment.ElementType.All)
                {
                    calcDMG = calcDMG * MG.ImmunityMultiplier;
                    calcDMGSize = calcDMGSize * MG.ImmunityMultiplier;
                }
                MG.TakeDamage(calcDMG);
                
            }

            ShowDMGText((int)calcDMG, calcDMGSize);
            if (WeaponEnabled)
            {
                Instantiate(equippedWeapon.popSpawn, collision.point, transform.rotation);
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

        playerEntity.rb2d.velocity = new Vector2(equippedWeapon.thrustDashDist * transform.localPosition.normalized.x, 0.6f * equippedWeapon.thrustDashDist * transform.localPosition.normalized.y);
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
    private float itemVelo;

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
    private void changeWeapVisibility()
    {
        if (PlayerPrefs.GetInt("weapIndicator", 0) == 0)
        {
            showWeapIndicator = false;
        }
        else if (PlayerPrefs.GetInt("weapIndicator", 0) == 1)
        {
            showWeapIndicator = true;
        }
        else
        {
            showWeapIndicator = false;
        }
    }

    private void ShowText(string text, float size, Color colour)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        GameObject gameObject2 = Instantiate(playerEntity.TextPopUp, position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = text;
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-30.0f, 30.0f));
        gameObject2.GetComponent<TMPro.TextMeshPro>().color = colour;
    }
}
