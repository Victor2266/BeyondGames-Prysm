using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    [SerializeField]
    private bool inMultiplayer = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Input.GetAxisRaw("Vertical") > 0f || Input.GetAxisRaw("Vertical") < 0f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Input.GetAxisRaw("Vertical") * collision.gameObject.GetComponent<PlayerEntity>().speed);
                if (!inMultiplayer)
                {
                    collision.gameObject.GetComponent<PlayerEntity>().isClimbing = true;
                }
                else
                {
                    collision.gameObject.GetComponent<NetworkPlayerController>().isClimbing = true;
                }
                collision.attachedRigidbody.gravityScale = 0f;
                //collision.gameObject.GetComponent<Animator>().SetBool("Climbing", true);
                //collision.gameObject.GetComponent<Animator>().speed = Mathf.Abs(Input.GetAxis("Vertical"));
            }
            else if (Input.GetAxisRaw("Vertical") == 0f)
            {
                if (!inMultiplayer)
                {
                    if (collision.gameObject.GetComponent<PlayerEntity>().isClimbing == true)
                    {
                        collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal") * collision.gameObject.GetComponent<PlayerEntity>().speed, Input.GetAxisRaw("Vertical") * collision.gameObject.GetComponent<PlayerEntity>().speed);
                        //collision.gameObject.GetComponent<Animator>().speed = 0;
                    }
                }
                else
                {
                    if (collision.gameObject.GetComponent<NetworkPlayerController>().isClimbing == true)
                    {
                        collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal") * collision.gameObject.GetComponent<PlayerEntity>().speed, Input.GetAxisRaw("Vertical") * collision.gameObject.GetComponent<PlayerEntity>().speed);
                        //collision.gameObject.GetComponent<Animator>().speed = 0;
                    }
                }
                
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.attachedRigidbody.gravityScale = 1f;
            if (!inMultiplayer)
            {
                collision.gameObject.GetComponent<PlayerEntity>().isClimbing = false;
            }
            else
            {
                collision.gameObject.GetComponent<NetworkPlayerController>().isClimbing = false;
            }
            //collision.gameObject.GetComponent<Animator>().speed = 1;
            //collision.gameObject.GetComponent<Animator>().SetBool("Climbing", false);
        }
    }
}
