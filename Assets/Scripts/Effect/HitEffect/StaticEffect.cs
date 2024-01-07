using UnityEngine;

public abstract class StaticEffect : AbstractEffect
{
    public float selfDestructTime = 10f;
    public float effectInterval = 3f;
    private float timer;

    private bool isUnactive = false;

    void Start()
    {
        timer = effectInterval;
    }

    public void UpdateEffect(Effector effector)
    {
        if(isUnactive)
        {
            effector.effectsRemoval.Add(this);
            Destroy(gameObject);
            return;
        }
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            UpdateEffectConcrete(effector);

            // Reset the timer
            timer = effectInterval;
        }

        // Add any other logic you need in the Update method
        // ...

        // Check if it's time to self-destruct
        selfDestructTime -= Time.deltaTime;
        if (selfDestructTime <= 0f)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        isUnactive = true;
    }

    protected abstract void UpdateEffectConcrete(Effector effector);
}
