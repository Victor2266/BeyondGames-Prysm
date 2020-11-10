using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkPlayerController : NetworkBehaviour
{
    public static float cameraSize = 3f;//camera size, these are public static because they change

    public static int Lives = 1;//number of lives, changes

    public static float speed = 3.8f;//max speed, changes (The value for single player is managed in my scene manager)

    private GameObject clone;//this game object contains the bullet, to position and impart velocity on it

    public static float jumpForce = 4.9f;//how high the player can jump

    public float fallMultiplier = 2.5f;// the gravity on a high jump is greater

    public float lowJumpMultiplier = 2f;//lower gravity for low jump

    private Rigidbody2D rb2d;// ridgidbody for physics
    private SpriteRenderer SprtRnderer;
    private Animator anim;//animation controller

    public GameObject Camera;//camera reference 
    public GameObject CameraOrbObj;

    private bool Flinch;//when the skeleton hits the player the player will flinch

    public Text LivesUI;//this is the text number for the lives in the ui
    public Text HealthUIText;
    public Text ManaUIText;

    public Slider health;//this is the health slider in ui

    public Slider mana;//slider for mana in UI

    private int ManaCost;//cost of the weapon the player is using in mana

    public GameObject transition;//the black transition object

    public static bool isDead;//if the player is dead

    public bool isDying;//if the player has 0 health and is dying

    private int charges = 1;//how many times the bullet shoots out

    public float timeStamp;//this calculates delta time for the cooldown of weapons
    private float SlideCooldown;

    public float coolDownPeriod;//cooldown val for weapons

    public int weapon;//weapon number
    
    public GameObject attack;//actual weapon prefab

    private float attackVelo;//velocity of weapon

    private bool rapid_fire;

    private bool power_control;

    private float inaccuracy;

    public GameObject[] weaponsList = new GameObject[14];


    public GameObject ChargeIndicator;//the particle system when you use a charged shot

    public static int[] Chargeable = new int[14];//array keeps track of which charged shots you've unlocked
    public GameObject Orb;
    public GameObject mousePointer = null;

    private CameraController OrbPosition;

    public GameObject gasPuff;
    public GameObject bloodPuff;
    private GameObject clone2;
    public GameObject redFlash;

    public GameObject cape;
    public bool isClimbing;

    public static int HealthBarScalingLength;

    public GameObject TextPopUp;

    private RectTransform component;//health slider rect transform
    private RectTransform component2;//mana slider rect transform
    
    public NetworkWeaponUI WeaponUI;

    private NetworkGamePlayerLobby NGPL;
    
    public bool inAir = false;

    public void EnterNewScene()
    {
        Start();
    }
    private void Start()
    {
        if (isServer)
        {
            if (!hasAuthority)
            {
                tag = "EnemyPlayer";
            }
        }
        if (!hasAuthority)
        {
            return;
        }
        tag = "Player";
        Camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (mousePointer == null)
        {
            mousePointer = GameObject.FindGameObjectWithTag("Cursor");
        }

        OrbPosition = Orb.GetComponent<CameraController>();

        NGPL = GetComponent<NetworkGamePlayerLobby>();
        SetWeap();
        ManaCost = 0;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SprtRnderer = GetComponent<SpriteRenderer>();
        component = health.GetComponent<RectTransform>();

        component2 = mana.GetComponent<RectTransform>();
        Camera.GetComponent<Camera>().orthographicSize = cameraSize;
        
        HealthBarScalingLength = Screen.width / 2;

        if (health.maxValue < NGPL.maxHealth)
        {
            if (NGPL.maxHealth < HealthBarScalingLength)
            {
                component.sizeDelta = new Vector2((float)NGPL.maxHealth, component.sizeDelta.y);
            }
            else
            {
                component.sizeDelta = new Vector2((float)HealthBarScalingLength, component.sizeDelta.y);
            }
            health.maxValue = (float)NGPL.maxHealth;
        }

        if (mana.maxValue < NGPL.maxMana)
        {
            if (NGPL.maxMana < HealthBarScalingLength)
            {
                component2.sizeDelta = new Vector2((float)NGPL.maxMana, component2.sizeDelta.y);
            }
            else
            {
                component2.sizeDelta = new Vector2((float)HealthBarScalingLength, component2.sizeDelta.y);
            }

            mana.maxValue = (int)NGPL.maxMana;
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        origin.y -= 0.6f;
        return Physics2D.Raycast(origin, -Vector2.up, 0.005f);
    }

    private void Update()
    {
        if (!hasAuthority)
            return;

        if (!isDying)
        {
            MoveUpdate();
            Shooting();
        }
        UpdateHealth();
        if (Input.GetButtonDown("Reset"))
        {
            TakeDamage(NGPL.currentHealth);
        }
    }

    public void SetWeap()//optimize this later to only run when pressed
    {

        if (weapon >= 1 && weapon <= 14)
        {
            //attack = weaponsList[weapon - 1];
            CmdSetAttack(weapon);

            attackVelo = weaponsList[weapon - 1].GetComponent<projectileController>().attackVelo;
            ManaCost = weaponsList[weapon - 1].GetComponent<projectileController>().ManaCost;
            coolDownPeriod = weaponsList[weapon - 1].GetComponent<projectileController>().coolDownPeriod;
            rapid_fire = weaponsList[weapon - 1].GetComponent<projectileController>().rapid_fire;
            power_control = weaponsList[weapon - 1].GetComponent<projectileController>().power_control;
            inaccuracy = weaponsList[weapon - 1].GetComponent<projectileController>().inaccuracy;
        }


        if (weapon == -1)//beginning light weapon
        {
            attackVelo = 2f;
            coolDownPeriod = 0.3f;
        }
    }

    public void Shooting()
    {
        if ((Input.GetButtonDown("Charge") || Input.GetMouseButtonDown(1)) && Chargeable[Mathf.Abs(weapon - 1)] == weapon)
        {
            if (weapon <= 7 && weapon > 0)
            {
                ChargeIndicator.SetActive(true);
                weapon += 7;
                Debug.Log("chrge up");
            }
            else if (weapon >= 8)
            {
                ChargeIndicator.SetActive(false);
                weapon -= 7;
                Debug.Log("chrge down");
            }
            SetWeap();
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (weapon >= 8)
            {
                ChargeIndicator.SetActive(false);
                weapon -= 7;
            }
        }

        if (timeStamp <= Time.time)
        {

            if ((Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(1)) ||
                (rapid_fire && (Input.GetMouseButton(0))))
                && charges == 1 && NGPL.currentMana - ManaCost >= 0)
            {
                WeaponUI.ScaleDown(coolDownPeriod);

                //clone = Instantiate(attack, transform.position, transform.rotation);
                //NetworkServer.Spawn(clone, connectionToClient);
                CmdServerSpawnProjectile(transform.position, transform.rotation);

                NGPL.CmdSetMana(NGPL.currentMana - ManaCost);
                //NGPL.CmdSetHealth(NGPL.currentHealth - ManaCost);
                charges = 0;
                if (weapon < 8)
                {
                    clone.GetComponent<projectileController>().Primed = false;
                }
            }
            if (charges == 0)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    if (clone != null)
                    {
                        Vector2 v = new Vector2(0.5f * Mathf.Cos(Mathf.Deg2Rad * (mousePointer.transform.eulerAngles.z - 90)), 0.5f * Mathf.Sin(Mathf.Deg2Rad * (mousePointer.transform.eulerAngles.z - 90)));
                        clone.transform.position = new Vector3(v.x + transform.position.x, v.y + transform.position.y, clone.transform.position.z);
                        clone.transform.eulerAngles = mousePointer.transform.eulerAngles;

                        if (power_control == false && inaccuracy == 0)
                        {
                            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(attackVelo * 2 * v.x, attackVelo * 2 * v.y);
                        }
                        else if (power_control && inaccuracy == 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, CameraOrbObj.transform.position);
                            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(attackVelo * 2 * v.x * scaled_pow, attackVelo * 2 * v.y * scaled_pow);
                        }
                        else if (power_control == false && inaccuracy > 0)
                        {
                            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(attackVelo * 2 * v.x - inaccuracy, attackVelo * 2 * v.x + inaccuracy),
                                Random.Range(attackVelo * 2 * v.y - inaccuracy, attackVelo * 2 * v.y + inaccuracy));
                        }
                        if (power_control && inaccuracy > 0)
                        {
                            float scaled_pow = Vector2.Distance(transform.position, CameraOrbObj.transform.position);
                            float velo = attackVelo * 2 * scaled_pow;
                            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(velo * v.x - inaccuracy, velo * v.x + inaccuracy),
                                Random.Range(velo * v.y - inaccuracy, velo * v.y + inaccuracy));
                        }
                        OrbPosition.offsetX = v.x;
                        OrbPosition.offsetY = v.y - 1.5f;

                    }

                }
                if (rapid_fire == true)
                {
                    timeStamp = Time.time + coolDownPeriod;
                    charges = 1;
                    clone.GetComponent<projectileController>().Primed = true;


                }
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    timeStamp = Time.time + coolDownPeriod;
                    charges = 1;
                    if (clone != null)
                    {

                        clone.GetComponent<projectileController>().Primed = true;
                    }


                }
            }
            else if (timeStamp + 2f <= Time.time)
            {
                OrbPosition.offsetX = 0.5f;
                OrbPosition.offsetY = -1f;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isLocalPlayer)
        {
            redFlash.SetActive(true);
        }
        //clone2 = Instantiate(bloodPuff, transform.position, transform.rotation);
        CmdServerSpawnBloodPuff(transform.position);
        NGPL.CmdSetHealth(NGPL.currentHealth - amount);
        if (NGPL.currentHealth <= 0f && !NetworkPlayerController.isDead)
        {
            StartCoroutine(DeathDelay(6f));
        }
    }

    public void UpdateHealth()
    {
        LivesUI.text = "Lives: " + NetworkPlayerController.Lives.ToString();

        HealthUIText.text = NGPL.currentHealth.ToString() + "/" + NGPL.maxHealth.ToString();
        HealthUIText.rectTransform.position = new Vector3(component.sizeDelta.x + 2, HealthUIText.rectTransform.position.y, HealthUIText.rectTransform.position.z);

        ManaUIText.text = NGPL.currentMana.ToString() + "/" + NGPL.maxMana.ToString();
        ManaUIText.rectTransform.position = new Vector3(component2.sizeDelta.x + 2, ManaUIText.rectTransform.position.y, ManaUIText.rectTransform.position.z);

        health.value = Mathf.SmoothStep(health.value, NGPL.currentHealth, 0.25f);
        mana.value = Mathf.SmoothStep(mana.value, NGPL.currentMana, 0.25f);
     
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "magicItem" && NGPL.currentMana < NGPL.maxMana)
        {
            NGPL.CmdSetMana(NGPL.currentMana + 2);
            if (NGPL.currentMana % 5 == 0 && NGPL.currentMana != NGPL.maxMana)
            {
                ShowText("+", 2, Orb.GetComponent<SpriteRenderer>().color);
            }
        }
        if (collision.tag == "healthItem" && NGPL.currentHealth < (float)NGPL.maxHealth)
        {
            NGPL.CmdSetHealth(NGPL.currentHealth + 2f);
            if ((int)NGPL.currentHealth % 5 == 0 && NGPL.currentHealth != NGPL.maxHealth)
            {
                ShowText("+", 2, Color.red);
            }
        }

        if (IsGrounded() && inAir == false)
        {
            inAir = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UpgradeHealth")
        {
            int amount = collision.GetComponent<UpgradeItem>().amount;
            collision.gameObject.SetActive(false);
            Upgrade(0, amount);
        }
        if (collision.tag == "UpgradeMana")
        {
            int amount2 = collision.GetComponent<UpgradeItem>().amount;
            Upgrade(1, amount2);
        }
        if (collision.tag == "UpgradeZoom")
        {
            int amount3 = collision.GetComponent<UpgradeItem>().amount;
            collision.gameObject.SetActive(false);
            Upgrade(2, amount3);
        }
        if (collision.tag == "UpgradeSpeed")
        {
            int amount4 = collision.GetComponent<UpgradeItem>().amount;
            collision.gameObject.SetActive(false);
            Upgrade(3, amount4);
        }

        if (IsGrounded() && inAir == false)
        {
            inAir = true;
        }
    }

    private IEnumerator DeathDelay(float interval)
    {
        transition.SetActive(true);
        if (!isDying)
        {
            anim.SetTrigger("Die");
            isDying = true;

            cape.SetActive(false);
        }
        yield return new WaitForSeconds(interval);
        NetworkPlayerController.isDead = true;
        yield break;
    }

    private IEnumerator JumpStretch()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.0001f);
            GetComponent<SpriteRenderer>().size = new Vector2(GetComponent<SpriteRenderer>().size.x - 0.02f, GetComponent<SpriteRenderer>().size.y + 0.02f);
        }
        for (int i = 0; i < 10; i++)
        {
            //yield return new WaitForSeconds(0.00001f);
            GetComponent<SpriteRenderer>().size = new Vector2(GetComponent<SpriteRenderer>().size.x + 0.02f, GetComponent<SpriteRenderer>().size.y - 0.02f);
        }
        yield break;
    }
    public void MoveUpdate()
    {
        if (rb2d.velocity.x > 0.5f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x - 0.3f, rb2d.velocity.y);
        }
        else if (rb2d.velocity.x < -0.5f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x + 0.3f, rb2d.velocity.y);
        }
        if (Input.GetAxisRaw("Horizontal") > 0f && Mathf.Abs(rb2d.velocity.x) < NetworkPlayerController.speed && !Flinch)
        {
            anim.SetBool("Running", true);
            rb2d.velocity = new Vector2(NetworkPlayerController.speed, rb2d.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Mathf.Abs(rb2d.velocity.x) < NetworkPlayerController.speed && !Flinch)
        {
            anim.SetBool("Running", true);
            rb2d.velocity = new Vector2(-NetworkPlayerController.speed, rb2d.velocity.y);
        }
        else if (Mathf.Abs(rb2d.velocity.x) < 0.05f)
        {
            anim.SetBool("Running", false);
        }

        if (rb2d.velocity.y < 0f && !isClimbing)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0f && !Input.GetButton("Jump") && !isClimbing)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                anim.SetTrigger("JumpTrigger");
                StartCoroutine(JumpStretch());
                Vector3 GasPos = new Vector3(transform.position.x, transform.position.y - 0.4f, 0);
                //clone2 = Instantiate(gasPuff, GasPos, transform.rotation);
                
                CmdServerSpawnGasPuff(GasPos);

                inAir = true;
            }
            else if (inAir)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                anim.SetTrigger("JumpTrigger");
                StartCoroutine(JumpStretch());
                Vector3 GasPos = new Vector3(transform.position.x, transform.position.y - 0.4f, 0);
                //clone2 = Instantiate(gasPuff, GasPos, transform.rotation);
                
                CmdServerSpawnGasPuff(GasPos);

                inAir = false;
            }
        }
        if (Input.GetButton("RegenMana") && NGPL.currentMana < (float)NGPL.maxMana && NGPL.currentHealth - 0.5f > 0)
        {
            NGPL.CmdSetHealth(NGPL.currentHealth - 0.5f);
            NGPL.CmdSetMana(NGPL.currentMana + 2);
        }
        if (Input.GetButtonDown("Slide") && IsGrounded())
        {
            if (SlideCooldown <= Time.time)
            {
                SlideCooldown = Time.time + 0.5f;

                anim.SetBool("Sliding", true);
                cape.SetActive(false);
                Flinch = true;
                rb2d.velocity = new Vector2(rb2d.velocity.x * 3f, rb2d.velocity.y);
                GetComponent<CapsuleCollider2D>().size = new Vector2(0.12f, 0.3f);
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0.01f, -0.36f);
            }

        }
        if (Input.GetButtonUp("Slide"))
        {
            anim.SetBool("Sliding", false);
            cape.SetActive(true);
            Flinch = false;
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.12f, 0.65f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0.01f, -0.16f);
        }
    }

    public void Flinching()
    {
        StartCoroutine(PauseAfterHit());
    }

    private IEnumerator PauseAfterHit()
    {
        Flinch = true;
        yield return new WaitForSeconds(0.2f);
        Flinch = false;
        yield break;
    }

    public void Upgrade(int type, int amount)
    {
        for (int i = amount; i > 0; i--)
        {
            if (type == 0)
            {
                NGPL.maxHealth++;
                NGPL.CmdSetHealth(NGPL.currentHealth + 1f);
            }
            if (type == 1)
            {
                NGPL.maxMana++;
                NGPL.CmdSetMana(NGPL.currentMana + 1);
            }
            if (type == 2)

            {
                NetworkPlayerController.cameraSize += 0.4f;
                Camera.GetComponent<Camera>().orthographicSize = NetworkPlayerController.cameraSize;
            }
            if (type == 3)
            {
                NetworkPlayerController.speed += 0.5f;
                NetworkPlayerController.jumpForce += 0.4f;
            }

            if (health.maxValue < NGPL.maxHealth)
            {
                if (NGPL.maxHealth < HealthBarScalingLength)
                {
                    component.sizeDelta = new Vector2((float)health.maxValue, component.sizeDelta.y);
                }
                else
                {
                    component.sizeDelta = new Vector2((float)HealthBarScalingLength, component.sizeDelta.y);
                }

                health.maxValue += 1;
            }

            if (mana.maxValue < NGPL.maxMana)
            {
                if (NGPL.maxMana < HealthBarScalingLength)
                {
                    component2.sizeDelta = new Vector2((float)mana.maxValue, component2.sizeDelta.y);
                }
                else
                {
                    component2.sizeDelta = new Vector2((float)HealthBarScalingLength, component2.sizeDelta.y);
                }

                mana.maxValue += 1;
            }
        }
    }

    private void ShowText(string text, float size, Color colour)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        GameObject gameObject2 = Instantiate(TextPopUp, position, transform.rotation);
        gameObject2.GetComponent<TMPro.TextMeshPro>().text = text;
        gameObject2.GetComponent<TMPro.TextMeshPro>().fontSize = size;
        gameObject2.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-30.0f, 30.0f));
        gameObject2.GetComponent<TMPro.TextMeshPro>().color = colour;
    }
    //NETWORKING
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Command]
    public void CmdServerSpawnGasPuff(Vector3 pos)
    {
        clone2 = Instantiate(gasPuff, pos, transform.rotation);
        NetworkServer.Spawn(clone2);
    }
    [Command]
    public void CmdServerSpawnBloodPuff(Vector3 pos)
    {
        clone2 = Instantiate(bloodPuff, pos, transform.rotation);
        NetworkServer.Spawn(clone2);
    }
    [Command]
    public void CmdServerSpawnProjectile(Vector3 pos, Quaternion rot)
    {
        clone = Instantiate(attack, pos, rot);
        NetworkServer.Spawn(clone, connectionToClient);
    }
    [Command]
    public void CmdSetAttack(int weapnum)
    {
        attack = weaponsList[weapnum - 1];
    }
}