using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour
{
    public MobGeneric parentAI;

    public void TakeDamage(float amount)
    {
        parentAI.TakeDamage(amount);
    }
    public void SetCollision()
    {

    }
}
