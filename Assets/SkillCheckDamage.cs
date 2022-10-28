using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckDamage : MonoBehaviour
{
    public bool isDead = false;
    public int minimimDamage;
    public DamageBar dmgBar;
    public void TakeDamage(float amount)
    {
        if (amount >= minimimDamage)
        {
            isDead = true;
        }
        dmgBar.UpdateBar(amount, minimimDamage);
    }

}
