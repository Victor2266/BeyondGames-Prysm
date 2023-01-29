using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followGameobject : MonoBehaviour
{
    public Transform followThis;
    public GameObject ignoreThis;

    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), ignoreThis.GetComponents<CapsuleCollider2D>()[0], true);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), ignoreThis.GetComponents<CapsuleCollider2D>()[1], true);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = followThis.position;
        transform.rotation = followThis.rotation;
    }
}
