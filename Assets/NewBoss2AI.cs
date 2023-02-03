using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoss2AI : MobGeneric
{

    public GameObject puppetStrings, redEyeSFX;
    public bool puppetMode;
    public OscillateUpDown Oscilater;

    private Vector3 velo;

    public Vector3 offsetFollow;
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
    }

    public void ActivatePuppetWarrior()
    {
        puppetMode = true;
        redEyeSFX.SetActive(true);
        puppetStrings.SetActive(true);
    }
}
