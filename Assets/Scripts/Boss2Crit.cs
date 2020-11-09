using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Crit : MonoBehaviour
{
    private void Start()
    {
        this.boss = GameObject.FindGameObjectWithTag("boss");
        Boss2AI component = this.boss.GetComponent<Boss2AI>();
        bossHealth = boss.GetComponent<HealthBarHealth>();
    }

    public void TakeDamage(float amount)
    {
        boss.GetComponent<Rigidbody2D>().gravityScale *= -1f;
        bossHealth.health -= amount;
    }

    public GameObject boss;
    private HealthBarHealth bossHealth;
}