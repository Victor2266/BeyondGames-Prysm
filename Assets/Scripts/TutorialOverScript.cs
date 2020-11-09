using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOverScript : MonoBehaviour
{
    public GameObject TutorialGuy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TutorialGuy.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
