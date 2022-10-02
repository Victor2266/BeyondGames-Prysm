using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
    public GameObject redEye;
    public Color Middle_colour;
    public Color Final_colour;

    public GameObject explosion;
    public GameObject bossBodyparts;
    public GameObject bossBlood;

    private bool ChangedToRed = false;
    public bool openingJaws = false;
    private float jaw_angle;

    public GameObject leftJaw;
    public GameObject rightJaw;
    private float jaw_speed = 0;

    public float leftWall;
    public float rightWall;
    public float topCiel;
    public float botFloor;
    public Transform eyeDirection;

    public bool activeLaser;
    private bool waitingForLaser=false;
    public GameObject laserBeam;
    private ParticleSystem laserPart;
    public GameObject LaserLight;
    private float _refLazerSize;
    private Vector3 _refvelo;

    public int topCount = 0;

    public Level1Manager levelManager;
    private void Start()
    {
        rb2d = base.GetComponent<Rigidbody2D>();
        healthObj = GetComponent<HealthBarHealth>();
        laserPart = laserBeam.GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        //when not laser
        if (!activeLaser)
        {
            laserPart.startSize = Mathf.SmoothDamp(laserPart.startSize, 0f, ref _refLazerSize, 0.1f);
            eyeDirection.gameObject.GetComponent<newPointerScript>().turn_speed = 2f;

            if (rb2d.position.x > rightWall)
            {
                velo = new Vector3(speed * Mathf.Cos(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), -speed * Mathf.Sin(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), 0f);
            }
            else if (rb2d.position.x < leftWall)
            {
                velo = new Vector3(speed * Mathf.Cos(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), -speed * Mathf.Sin(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), 0f);
            }
            if (rb2d.position.y > topCiel)
            {
                topCount++;
                velo = new Vector3(speed * Mathf.Cos(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), speed * Mathf.Sin(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), 0f);
            }
            else if (rb2d.position.y < botFloor)
            {
                velo = new Vector3(speed * Mathf.Cos(Mathf.Deg2Rad * (eyeDirection.eulerAngles.z - 90f)), speed, 0f);
                topCiel = Random.RandomRange(6f, 10f);

            }
            rb2d.velocity = new Vector2(velo.x, velo.y);

            
        }
        
        //when laser
        else if (activeLaser)
        {
            eyeDirection.gameObject.GetComponent<newPointerScript>().turn_speed = 0.5f;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x, 4f, 0f), ref _refvelo, 1f);
            rb2d.velocity = new Vector2(0f, 0f);
            laserBeam.SetActive(true);

            if(healthObj.health <= 100f)
            {
                laserPart.startSize = Mathf.SmoothDamp(laserPart.startSize, 0.5f, ref _refLazerSize, 1f);
                if (!waitingForLaser)
                {
                    waitingForLaser = true;
                    GetComponent<AudioSource>().Play();
                    laserBeam.GetComponent<AudioSource>().volume = 0.5f;
                    laserBeam.GetComponent<AudioSource>().Play();
                    LaserLight.SetActive(true);
                    StartCoroutine(waitForLaser(3f));
                }
                    
            }else if (healthObj.health <= 200f)
            {
                laserPart.startSize = Mathf.SmoothDamp(laserPart.startSize, 0.5f, ref _refLazerSize, 2f);
                if (!waitingForLaser)
                {
                    waitingForLaser = true;
                    GetComponent<AudioSource>().Play();
                    laserBeam.GetComponent<AudioSource>().volume = 0.25f;
                    laserBeam.GetComponent<AudioSource>().Play();
                    LaserLight.SetActive(true);
                    StartCoroutine(waitForLaser(4f));
                }
            }
            else
            {
                laserPart.startSize = Mathf.SmoothDamp(laserPart.startSize, 0.5f, ref _refLazerSize, 2f);
                if (!waitingForLaser)
                {
                    waitingForLaser = true;
                    GetComponent<AudioSource>().Play();
                    laserBeam.GetComponent<AudioSource>().volume = 0.25f;
                    laserBeam.GetComponent<AudioSource>().Play();
                    LaserLight.SetActive(true);
                    StartCoroutine(waitForLaser(4.5f));
                }
            }
        }


        //always
        if (healthObj.health <= 0f && !isDead)
        {
            Death();
        }
        else if (healthObj.health <= 100f)
        {
            speed = 12f;
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Final_colour, 0.005f);
            if (ChangedToRed == false)
            {
                GetComponent<AudioSource>().Play();
                redEye.SetActive(true);
                ChangedToRed = true;
            }
            if (topCount >= 4)
            {
                topCount = 0;
                var veloOverLife = laserPart.velocityOverLifetime;
                veloOverLife.orbitalY = Random.Range(9f,16f);
                activeLaser = true;
            }
        }
        else if (healthObj.health <= 200f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Middle_colour, 0.005f);
            speed = 8f;
            if (topCount >= 3)
            {
                topCount = 0;
                activeLaser = true;
            }
        }
        else
        {
            if (topCount >= 5)
            {
                topCount = 0;
                activeLaser = true;
            }
        }

        if (openingJaws)
        {
            jaw_angle = Mathf.SmoothDamp(leftJaw.GetComponent<Transform>().localEulerAngles.z, 15, ref jaw_speed, 0.5f);
        }
        else
        {
            jaw_angle = Mathf.SmoothDamp(leftJaw.GetComponent<Transform>().localEulerAngles.z, 75, ref jaw_speed, 0.05f);
            if (leftJaw.GetComponent<Transform>().localEulerAngles.z > 74)
            {
                openingJaws = true;
            }
        }
        leftJaw.GetComponent<Transform>().localEulerAngles = new Vector3(0f, 0f, jaw_angle);
        rightJaw.GetComponent<Transform>().localEulerAngles = new Vector3(0f, 0f, -jaw_angle);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x * 10f, rb2d.velocity.y * 4f);
            player.SendMessage("TakeDamage", 20);
            openingJaws = false;
            chompsound.Play();
        }
        if (collision.gameObject.tag == "platform")
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision, true);
        }
    }

    public void TakeDamage(float amount)
    {
        mobTimer++;
        openingJaws = false;
        if (mobTimer > 10)
        {
            mobTimer -= 10;
            if (healthObj.health < 60f)
            {
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            else if (healthObj.health < 120f)
            {
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                clone = UnityEngine.Object.Instantiate<GameObject>(MobDrop, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
        }
        manaTimer++;
        if (manaTimer > 5)
        {
            manaTimer -= 5;
            clone = UnityEngine.Object.Instantiate<GameObject>(ManaDrops, base.transform.position, base.transform.rotation);

        }
        healthObj.health -= amount;
        healthBar.UpdateHealthBar(healthObj.health, 350f);
        GetComponent<AudioSource>().Play();

        Instantiate<GameObject>(bossBlood, base.transform.position, base.transform.rotation);
    }

    private void Death()
    {
        isDead = true;
        Instantiate<GameObject>(explosion, base.transform.position, base.transform.rotation);
        Instantiate<GameObject>(explosion, base.transform.position, base.transform.rotation);
        Instantiate<GameObject>(explosion, base.transform.position, base.transform.rotation);
        base.gameObject.SetActive(false);
        Instantiate<GameObject>(ExtraNeon, base.transform.position, base.transform.rotation);
        Instantiate<GameObject>(bossBodyparts, base.transform.position, base.transform.rotation);

        levelManager.bossSoul = Instantiate<GameObject>(soul, new Vector3(transform.position.x, transform.position.y, -2f), base.transform.rotation);
        if(BossRoomRange != null)
        {

            BossRoomRange.GetComponent<AudioSource>().enabled = false;
        }
        healthBar.gameObject.SetActive(false);
    }
    private IEnumerator waitForLaser(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        activeLaser = false;
        waitingForLaser = false;
        LaserLight.SetActive(false);
    }
    public Vector2 velo = new Vector2(2f, 2f);

    public GameObject BossRoomRange;
    public GameObject player;
    public GameObject ManaDrops;
    public GameObject soul;
    public GameObject HealthDrops;
    public AudioSource chompsound;
    public GameObject MobDrop;
    public HealthBarHealth healthObj;
    public HealthBar healthBar;

    public GameObject ExtraNeon;

    public float speed = 2f;

    private GameObject clone;

    private int mobTimer;

    private int manaTimer;

    private Rigidbody2D rb2d;

    private bool isDead;
    
}