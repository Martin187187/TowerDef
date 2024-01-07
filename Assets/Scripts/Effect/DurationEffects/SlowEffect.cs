using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : EnemyStatsEffect
{
    public float slowRatio = 0.1f;

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {

        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        SlowEffect otherEffect = (SlowEffect)otherObject;
        slowRatio *= otherEffect.slowRatio;
        Destroy(otherEffect.gameObject);
    }

    public override float GetAdditionValue(Enemy effector)
    {
        return 0;
    }

    public override float GetMultiplicationValue(Enemy effector)
    {
        return slowRatio;
    }
}