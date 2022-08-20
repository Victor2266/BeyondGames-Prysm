using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratPoly : MonoBehaviour
{
    public Vector3 initalVelo;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(initalVelo.x, initalVelo.y, initalVelo.z); 
    }
}
