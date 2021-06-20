using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbControler : MonoBehaviour
{
    public GameObject pop;
    public GameObject bigPop;
    private GameObject clone;

    public GameObject Player;
    private PlayerController playerScript;
    public SpriteRenderer SprtRndr;

    private void Start()
    {
        playerScript = Player.GetComponent<PlayerController>();
        SprtRndr = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal Shooting") || Input.GetButtonDown("Vertical Shooting") && Player.GetComponent<PlayerEntity>().weapon < 8)
        {
            clone = Instantiate<GameObject>(pop, transform.position, transform.rotation);
        }
    }
}
