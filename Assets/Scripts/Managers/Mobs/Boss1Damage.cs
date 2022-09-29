﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Damage : MonoBehaviour
{
    private void Start()
    {
        this.boss = GameObject.FindGameObjectWithTag("boss");
        bossHealth = boss.GetComponent<HealthBarHealth>();
    }

    public void TakeDamage(float amount) {
        this.clone = UnityEngine.Object.Instantiate<GameObject>(this.HealthDrops, base.transform.position, base.transform.rotation);
        boss.GetComponent<Boss1AI>().openingJaws = false;
        bossHealth.health -= amount;

        healthBar.UpdateHealthBar(bossHealth.health, 350f);

        Instantiate<GameObject>(boss.GetComponent<Boss1AI>().bossBlood, base.transform.position, base.transform.rotation);
    }

    public HealthBar healthBar;

    public GameObject boss;

    private GameObject clone;

    public GameObject HealthDrops;

    private HealthBarHealth bossHealth;
}