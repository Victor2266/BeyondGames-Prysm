﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamagePlayer : MonoBehaviour
{
    public ParticleSystem part;
    public float minSize;
    public float DMG = 1f;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (part.startSize > minSize)
        {
            if (other.tag == "Player")
                other.GetComponent<PlayerManager>().TakeDamage(DMG);
        }
    }
}
