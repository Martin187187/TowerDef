using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public float selfDestructTime = 10f; // Set the time after which the GameObject should be destroyed
    public float effectParentInterval = 3f; // Set the interval for affecting the parent GameObject

    void Start()
    {
        // InvokeRepeating the abstract method to affect the parent every y seconds
        InvokeRepeating("EffectParent", 0f, effectParentInterval);

        // Invoke the method to self-destruct after x seconds
        Invoke("DestroySelf", selfDestructTime);
    }

    // Abstract method to be implemented by derived classes
    protected abstract void EffectParent();

    void DestroySelf()
    {
        // Stop the repeating invocation before destroying the GameObject
        CancelInvoke("EffectParent");

        // Destroy the current GameObject
        Destroy(gameObject);
    }
}

