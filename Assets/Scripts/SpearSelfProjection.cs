using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSelfProjection : MonoBehaviour
{
    private GameObject clone;
    public GameObject SpearProjection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float parentVelocity = transform.parent.GetComponentInParent<Rigidbody2D>().velocity.x;

            clone = Instantiate(SpearProjection, transform.position, transform.rotation);
            clone.transform.localScale = new Vector3(transform.parent.GetComponentInParent<Transform>().localScale.x, 1f, 1f);
            clone.GetComponent<Rigidbody2D>().velocity = new Vector3(parentVelocity * 1.5f, 0f, 0f);
            
            if (0 < parentVelocity && parentVelocity < 0.1f)
            {
                clone.GetComponent<Rigidbody2D>().velocity = new Vector3(2f, 0f, 0f);
            }
            else if (0 > parentVelocity && parentVelocity > -0.1f)
            {
                clone.GetComponent<Rigidbody2D>().velocity = new Vector3(-2f, 0f, 0f);
            }
        }
    }
}
