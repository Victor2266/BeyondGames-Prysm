using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplasher : MonoBehaviour
{
    public GameObject WaterSplash;


    private void OnCollisionEnter2D(Collision2D collision)
    {

        Instantiate(WaterSplash, collision.contacts[0].point, WaterSplash.transform.rotation);

    }
}
