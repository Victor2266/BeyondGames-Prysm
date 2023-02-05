using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoss2AI : MobGeneric
{

    public GameObject puppetStrings, redEyeSFX, bodyParticles;
    public bool puppetMode;
    public OscillateUpDown Oscilater;

    private Vector3 velo;

    public Vector3 offsetFollow;
    public bool followPlayer;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
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
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offsetFollow, ref velo, 2f);
        }
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
        puppetMode = false;
        bodyParticles.GetComponent<ParticleSystem>().gravityModifier = 0f;
        redEyeSFX.SetActive(false);
        puppetStrings.SetActive(false);
        followPlayer = true;
        textYOffset = -0.93f;
        offsetFollow = new Vector3(offsetFollow.x, offsetFollow.y, -2f);
    }
}
