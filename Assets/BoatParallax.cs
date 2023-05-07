using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatParallax : MonoBehaviour
{
    public float leftWorldSpace, rightWorldSpace;
    public float leftLimit, rightLimit;
    private float movementLimit;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        movementLimit = rightLimit - leftLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.x < leftWorldSpace)
        {
            transform.localPosition = new Vector3(leftLimit, transform.localPosition.y, transform.localPosition.z);
        }
        else if (player.position.x > rightWorldSpace)
        {
            transform.localPosition = new Vector3(rightLimit, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            float percent = (player.transform.position.x - leftWorldSpace) / (rightWorldSpace - leftWorldSpace);
            transform.localPosition = new Vector3(percent* movementLimit - rightLimit, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
