using UnityEngine;
public abstract class AbstractStatsEffect<T> : MonoBehaviour where T : Effector{


    public abstract float getAdditionValue(T effector);
    public abstract float getMultiplicationValue(T effector);
}