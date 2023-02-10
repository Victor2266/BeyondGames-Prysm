using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : HasWeakness
{
    public MobGeneric parentAI;

    public override void TakeDamage(float amount)
    {
        parentAI.TakeDamage(amount);
    }
    public void SetCollision()
    {

    }
}
