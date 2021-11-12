using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToXVelocity : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(-rb2d.velocity.x / Mathf.Abs(rb2d.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
