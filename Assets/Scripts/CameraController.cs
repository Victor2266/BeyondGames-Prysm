using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{   //depreciated, use the delayed camera controller instead
    public Vector2 velocity;

    public bool notTrackingPlayer;

    public float smoothTimeY;

    public float smoothTimeX;

    public float offsetX;

    public float offsetY;

    public GameObject player;

    private void Start()
    {
        if (notTrackingPlayer == false)
        {
            //Only delays when tracking player
            StartCoroutine(waitForStart());
        }
    }

    private void FixedUpdate()
    {
        float x = Mathf.SmoothDamp(transform.position.x, player.transform.position.x + offsetX, ref velocity.x, smoothTimeX);
        float num = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + offsetY, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(x, num + 0.5f, transform.position.z);
        if (notTrackingPlayer == false)
        {
            if (player.GetComponent<Rigidbody2D>().velocity.x > 0.5f)
            {
                offsetX = player.GetComponent<PlayerEntity>().speed / 2f;
            }
            if (player.GetComponent<Rigidbody2D>().velocity.x < -0.5f)
            {
                offsetX = -player.GetComponent<PlayerEntity>().speed / 2f;
            }
        }
    }
    private IEnumerator waitForStart()
    {
        yield return new WaitForSeconds(1.9f);
        player = GameObject.FindGameObjectWithTag("Player");
    }
}