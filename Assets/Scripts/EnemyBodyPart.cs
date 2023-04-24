using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : HasWeakness
{
    public MobGeneric parentAI;

    public override void TakeDamage(float amount)
    {
        parentAI.TakeDamage(amount);

        LeanTween.cancel(gameObject);
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.color(gameObject, Color.red, 0.1f).setEaseInOutSine());
        sequence.append(LeanTween.color(gameObject, Color.white, 0.1f).setEaseInOutSine());
    }
    public void SetCollision()
    {

    }
}
