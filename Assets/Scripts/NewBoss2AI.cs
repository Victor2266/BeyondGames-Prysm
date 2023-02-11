using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class NewBoss2AI : MobGeneric
{

    public GameObject puppetStrings, redEyeSFX, bodyParticles;
    public bool puppetMode;
    public OscillateUpDown Oscilater;

    private Vector3 velo;
    private float _orbitalSize;
    private float orbitalSizeTarget;

    public Vector3 offsetFollow;
    public bool followPlayer;
    public GameObject player;

    private int laserCounter, skeletonCounter, dragCounter;
    private bool isSwinging, activeLaser, waitingForLaser, isDragging;

    public GameObject eyeLeft, eyeRight;

    public newPointerScript eyeLeftPointer, eyeRightPointer;

    public GameObject laser1, laser2;
    public ParticleSystem laserPart1, laserPart2;
    private float _refLazerSize;
    public GameObject[] indicatorLights;
    public AudioSource laserAudio;

    public SkeletonSpawner skeletonSpawner;

    ParticleSystem.VelocityOverLifetimeModule lPV1;
    ParticleSystem.VelocityOverLifetimeModule lPV2;
    public bool agression;

    public enemyWeapon weapon;
    public float playerFollowSpeed;

    // Start is called before the first frame update
    void Start()
    {

        eyeLeftPointer = eyeLeft.GetComponent<newPointerScript>();
            eyeRightPointer = eyeRight.GetComponent<newPointerScript>();


        lPV1 = laserPart1.velocityOverLifetime;
        lPV2 = laserPart2.velocityOverLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (puppetMode)
        {
            transform.position = Vector3.SmoothDamp(transform.position, puppetStrings.transform.position + offsetFollow, ref velo, 1f);
            Oscilater.enabled=(false);
        }
        else if (followPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offsetFollow, ref velo, playerFollowSpeed);
            if (agression)
            {
                if (!activeLaser)
                {
                    laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 0f, ref _refLazerSize, 0.5f);
                    laserPart2.startSize = laserPart1.startSize;


                    eyeLeftPointer.turn_speed = 2f;
                    eyeRightPointer.turn_speed = 2f;


                    if (laserCounter == 10)
                    {
                        StartCoroutine(activateLaser(10f));
                        laserCounter = 0;
                    }
                    else if(skeletonCounter == 2)
                    {
                        skeletonSpawner.Spawn();
                        skeletonCounter = 0;
                    }
                    else if(dragCounter == 3 && !isSwinging)
                    {
                        isDragging = true;
                        dragCounter = 0;
                        if(Random.Range(0,2) > -1)
                        {
                            playerFollowSpeed = 0.5f;
                            audioSource.Play();
                            if (weapon.LookingLeft == true)
                            {
                                anim.SetTrigger("DraggingRight");
                                StartCoroutine(DragScythe(true));
                            }
                            else
                            {

                                anim.SetTrigger("DraggingLeft");
                                StartCoroutine(DragScythe(false));
                            }
                        }
                    }
                    else if (isDragging)
                    {
                        if (weapon.LookingLeft == true)
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                        }
                        else
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                    }
                    
                }
                else if (activeLaser)
                {
                    ShootingLaser();
                }

                if(transform.position.x > player.transform.position.x)
                {
                    weapon.LookingLeft = true;
                }
                else
                {
                    weapon.LookingLeft = false;
                }

            }
            

            //set laser orbital size
            orbitalSize = Mathf.SmoothDamp(orbitalSize, orbitalSizeTarget, ref _orbitalSize, 1f);
            lPV1.orbitalX = orbitalSize;
            lPV2.orbitalX = lPV1.orbitalX;
        }
    }

    private float orbitalSize;
    private void ShootingLaser()
    {

        if (Health <= 100f)
        {
            laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 1f, ref _refLazerSize,2f);
            laserPart2.startSize = laserPart1.startSize;


            if (!waitingForLaser)
            {
                eyeLeftPointer.turn_speed = 0.5f;
                eyeRightPointer.turn_speed = 0.5f;
                waitingForLaser = true;
                GetComponent<AudioSource>().Play();
                laserAudio.volume = 0.5f;
                laserAudio.Play();
            }

        }
        else if (Health <= 200f)
        {
            laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 1f, ref _refLazerSize, 2f);
            laserPart2.startSize = laserPart1.startSize;
            if (!waitingForLaser)
            {
                eyeLeftPointer.turn_speed = 0.5f;
                eyeRightPointer.turn_speed = 0.5f;
                waitingForLaser = true;
                GetComponent<AudioSource>().Play();
                laserAudio.volume = 0.5f;
                laserAudio.Play();
            }
        }
        else
        {
            laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 1f, ref _refLazerSize, 2f);
            laserPart2.startSize = laserPart1.startSize;
            if (!waitingForLaser)
            {
                eyeLeftPointer.turn_speed = 0.5f;
                eyeRightPointer.turn_speed = 0.5f;
                waitingForLaser = true;
                GetComponent<AudioSource>().Play();
                laserAudio.volume = 0.5f;
                laserAudio.Play();
            }
        }
        
    }

    private IEnumerator activateLaser(float seconds)
    {
        indicatorLights[0].SetActive(true);
        indicatorLights[1].SetActive(true);

        yield return new WaitForSeconds(7.1f);

        eyeLeftPointer.turn_speed = 0.5f;
        eyeRightPointer.turn_speed = 0.5f;
        laser1.SetActive(true);
        laser2.SetActive(true);
        activeLaser = true;

        orbitalSizeTarget = 8f;
        playerFollowSpeed = 3.5f;

        yield return new WaitForSeconds(seconds);
        activeLaser = false;
        waitingForLaser = false;
        indicatorLights[0].SetActive(false);
        indicatorLights[1].SetActive(false);

        orbitalSizeTarget = 0.1f;
        playerFollowSpeed = 2f;
    }

    private IEnumerator DragScythe(bool lookingLeft)
    {
        playerFollowSpeed = 0.5f;
        if (lookingLeft) {
            offsetFollow = new Vector3(5f, 3f, -2f);
        }
        else
        {
            offsetFollow = new Vector3(-5f, 3f, -2f);
        }
        yield return new WaitForSeconds(2f);
        if (lookingLeft)
        {
            offsetFollow = new Vector3(-7f, 3f, -2f);
        }
        else
        {
            offsetFollow = new Vector3(7f, 3f, -2f);
        }

        yield return new WaitForSeconds(1f);
        isDragging = false;
        offsetFollow = new Vector3(0, 5f, -2f);

        anim.SetTrigger("Idle");

    }

    public void TakeDamage(float amount)
    {
        laserCounter++;
        skeletonCounter++;
        dragCounter++;

        if (skeletonCounter > 5)
        {
            skeletonCounter -= 5;
            skeletonSpawner.Spawn();
        }
        Health -= amount;
        healthBar.UpdateHealthBar(Health, 500f);
        audioSource.Play();

        BSplat.Spray((int)amount / 2);

        if(Health <= 0f && !isDead)
        {
            Death();
        }
    }

    protected override void Death()
    {
        isDead = true;
        clone = Instantiate(DeathItem, new Vector3(transform.position.x, transform.position.y, -1f), base.transform.rotation);
       
        //base.gameObject.GetComponentInChildren<Light>().enabled = false;
        anim.SetTrigger("dead");
        anim.SetBool("FullyDead", true);
        healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);

        CameraShaker.Instance.ShakeOnce(15f, 10f, 0f, 10f);
        //Destroy(healthBar.gameObject);
    }


    public void ActivatePuppetWarrior()
    {
        puppetMode = true;
        bodyParticles.GetComponent<ParticleSystem>().gravityModifier = 0.1f;
        redEyeSFX.SetActive(true);
        puppetStrings.SetActive(true);
    }
    public void DeactivatePuppetWarrior()
    {
        playerFollowSpeed = 2f;
        puppetMode = false;
        bodyParticles.GetComponent<ParticleSystem>().gravityModifier = 0f;
        redEyeSFX.SetActive(false);
        puppetStrings.SetActive(false);
        followPlayer = true;
        textYOffset = -0.93f;
        offsetFollow = new Vector3(offsetFollow.x, offsetFollow.y, -2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSwinging && !activeLaser && agression)
        {
            if(collision.tag == "Player")
            {
                if(player.transform.position.y - transform.position.y < -4.5f && !isDragging)
                {
                    isSwinging = true;
                    offsetFollow = new Vector3(0, 3f, -2f);
                    anim.SetTrigger("QuickOneTwo");
                    playerFollowSpeed = 1f;

                    skeletonCounter++;
                    laserCounter++;
                    dragCounter++;
                }
            }
        }
    }

    public void DoneSwinging()
    {
        isSwinging = false;
        if (!isDragging)
        {
            offsetFollow = new Vector3(0, 5f, -2f);
            playerFollowSpeed = 2f;
        }
    }
}
