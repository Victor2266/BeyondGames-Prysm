using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOn_Machine : MonoBehaviour
{
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetButtonDown("Use/Interact") && closed)
        {
            closed = false;
            item.SetActive(true);
            anim.SetTrigger("turnOn");
        }
    }

    public GameObject item;

    public bool closed = true;

    private Animator anim;
    
}
