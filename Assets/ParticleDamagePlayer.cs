using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamagePlayer : MonoBehaviour
{
    public ParticleSystem part;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
            other.GetComponent<PlayerManager>().TakeDamage(1f);
    }
}
