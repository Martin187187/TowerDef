using UnityEngine;
public class IncreaseStatsEffect : Effect
{
    public enum Type {
        ATTACK, ATTACK_SPEED, ROTATION_SPEED, RANGE
    }
    public enum Integration {
        ADDITION, MULTIPLICATION
    }

    public Type type;
    public Integration integration;
    public float amount;

    protected override void EffectParent()
    {
    }
}
