using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ElevatorDoorsOpening : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public float openPos;
    public float closedPos;
    public float speed;
    private float posX;

    private Rigidbody2D rb2d;
    public bool opening;
    private Transform door;
    public bool shaking = false;

    public GameObject sparkExplosion;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        door = rightDoor.GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "destroyoncontact")
        {
            if (Mathf.Abs(door.localPosition.x) - closedPos < 0.01f)
            {
                if (opening == false) {
                    GameObject clone;
                    clone = Instantiate(sparkExplosion, new Vector3(transform.position.x, transform.position.y - 6.5f, transform.position.z) , transform.rotation);
                }
                opening = true;
            }
        }
        if (collision.tag == "Finish")
        {
            if (!shaking)
            {
                shaking = true;
                CameraShaker.Instance.ShakeOnce(8, 3, .1f, 1.2f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Mathf.Abs(door.localPosition.x) - openPos < 0.01f)
            {
                opening = false;
                rb2d.gravityScale = -1f;
                GetComponents<BoxCollider2D>()[0].enabled = false;
                GetComponents<BoxCollider2D>()[1].enabled = false;
                GetComponent<DeathTimer>().enabled = true;
            }

        }
    }

    private void Update()
    {
        if (opening) {
            posX = Mathf.SmoothDamp(door.localPosition.x, openPos, ref speed, 0.5f);
        }
        else
        {
            posX = Mathf.SmoothDamp(door.localPosition.x, closedPos, ref speed, 0.5f);
        }
        leftDoor.GetComponent<Transform>().localPosition = new Vector3(-posX, -0.2581787f, 0f);
        rightDoor.GetComponent<Transform>().localPosition = new Vector3(posX, -0.2581787f, 0f);
    }
}
