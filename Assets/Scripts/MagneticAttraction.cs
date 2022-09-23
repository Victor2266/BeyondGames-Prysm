using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAttraction : MonoBehaviour
{

    private Vector3 _velo;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.position = Vector3.SmoothDamp(transform.position, collision.transform.position, ref _velo, 0.5f);
        }
    }
}
