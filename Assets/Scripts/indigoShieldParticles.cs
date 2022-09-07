using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indigoShieldParticles : MonoBehaviour
{
    public GameObject particle;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemyProj")
        {
            Instantiate(particle, transform, false);
        }
    }
}
