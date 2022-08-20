using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnCollisionEnter : MonoBehaviour
{
    public GameObject SpawnedObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(SpawnedObject, transform.position, transform.rotation);
    }
}
