using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeedEffect : ProjectileStatsEffect
{
    public float speedRatio = 1.2f;
    public float exponent = 2f;
    
    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {

        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        IncreaseSpeedEffect otherEffect = (IncreaseSpeedEffect)otherObject;
        Destroy(otherEffect.gameObject);
    }
    public override float GetAdditionValue(Projectile effector)
    {
        float x = effector.GetLifetime();
        return speedRatio * Mathf.Pow(x, exponent);
    }

    public override float GetMultiplicationValue(Projectile effector)
    {
        return 1;
    }
}