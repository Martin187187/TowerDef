using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : StaticEffect
{
    public float damageRatio = 0.05f;

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {

        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        PoisonEffect otherEffect = (PoisonEffect)otherObject;
        damageRatio += otherEffect.damageRatio;
        Destroy(otherEffect.gameObject);
    }

    protected override void UpdateEffectConcrete(Effector effector)
    {
        EffectParent((Enemy)effector);
    }
    private void EffectParent(Enemy enemy)
    {
        enemy.Damage((int)(enemy.hp * damageRatio));
    }

}