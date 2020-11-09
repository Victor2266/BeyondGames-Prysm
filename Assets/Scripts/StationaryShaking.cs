using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryShaking : MonoBehaviour
{
    public bool shakeX;
    public bool shakeY;
    public Vector2 magnitude;
    private Vector3 oriPosition;

    // Start is called before the first frame update
    void Start()
    {
        oriPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeX == true)
        {
            transform.position = new Vector3(oriPosition.x + Random.Range(0, magnitude.x), transform.position.y, oriPosition.z);
        }
        if (shakeY == true)
        {
            transform.position = new Vector3(transform.position.x, oriPosition.y + Random.Range(0, magnitude.y), oriPosition.z);
        }
    }
}
