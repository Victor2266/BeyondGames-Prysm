using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeAcceleration : MonoBehaviour
{
    public GameObject CapeObj;
    private Cloth clothComponent;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        clothComponent = CapeObj.GetComponent<Cloth>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        clothComponent.externalAcceleration = new Vector3(rb2d.velocity.x * -2f , 4f, 0f);
    }
}
