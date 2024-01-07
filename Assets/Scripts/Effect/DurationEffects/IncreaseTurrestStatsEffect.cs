using UnityEngine;
public class IncreaseTurretStatsEffect : TurrestStatsEffect
{
    public float addition = 10;
    public float multiplication = 1;

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {

        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        IncreaseTurretStatsEffect otherEffect = (IncreaseTurretStatsEffect)otherObject;
        addition += otherEffect.addition;
        multiplication *= otherEffect.multiplication;
        Destroy(otherEffect.gameObject);
    }

    public override float GetAdditionValue(Turret effector)
    {
        return addition;
    }

    public override float GetMultiplicationValue(Turret effector)
    {
        return multiplication;
    }
}