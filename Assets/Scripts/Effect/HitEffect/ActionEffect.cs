using UnityEngine;

public abstract class ActionEffect<T> : MonoBehaviour where T : Effector{
    
    public abstract bool OnHitEffect(T effected);
    public abstract void UpdateEffect();
}