using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private void Start()
    {
        healthScript = Mob.GetComponent<HealthBarHealth>();
        
        CurrentHealth = healthScript.health;
        
    }

    private void Update()
    {
        if (CurrentHealth > healthScript.health + 1 )
        {
            CurrentHealth -= 2f;
        }
        else if (CurrentHealth < healthScript.health - 1)
        {
            CurrentHealth += 2f;
        }
        base.transform.localScale = new Vector3(CurrentHealth / divisor, base.transform.localScale.y, base.transform.localScale.z);
        if (healthScript.health / divisor <= 0f)
        {
            base.transform.localScale = new Vector3(0f, base.transform.localScale.y, base.transform.localScale.z);
        }
    }

    public GameObject Mob;

    public float divisor;

    private float CurrentHealth;

    private HealthBarHealth healthScript;
}