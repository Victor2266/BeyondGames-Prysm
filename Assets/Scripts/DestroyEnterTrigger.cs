using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnterTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "destroyoncontact")
        {
            Destroy(gameObject);
        }
    }
}
