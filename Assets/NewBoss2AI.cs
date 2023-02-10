using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int laserCounter, skeletonCounter;
    private bool activeLaser, waitingForLaser;

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
                    playerFollowSpeed = 2f;
                    laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 0f, ref _refLazerSize, 0.5f);
                    laserPart2.startSize = laserPart1.startSize;


                    eyeLeftPointer.turn_speed = 2f;
                    eyeRightPointer.turn_speed = 2f;

                    if (indicatorLights[0].activeSelf == false)
                    {

                        StartCoroutine(activateLaser(10f));
                        skeletonSpawner.Spawn();
                    }
                }
                else if (activeLaser)
                {
                    ShootingLaser();
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
        playerFollowSpeed = 3.5f;

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
            laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 1f, ref _refLazerSize, 1.5f);
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
            laserPart1.startSize = Mathf.SmoothDamp(laserPart1.startSize, 1f, ref _refLazerSize, 1f);
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

        yield return new WaitForSeconds(seconds);
        activeLaser = false;
        waitingForLaser = false;
        indicatorLights[0].SetActive(false);
        indicatorLights[1].SetActive(false);

        orbitalSizeTarget = 0.1f;
    }

    public void TakeDamage(float amount)
    {
        laserCounter++;
        skeletonCounter++;

        if (skeletonCounter > 5)
        {
            skeletonCounter -= 5;
            skeletonSpawner.Spawn();
        }
        Health -= amount;
        healthBar.UpdateHealthBar(Health, 500f);
        audioSource.Play();

        BSplat.Spray((int)amount / 2);
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
}
