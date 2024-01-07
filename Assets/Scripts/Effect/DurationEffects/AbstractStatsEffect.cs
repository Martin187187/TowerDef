using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractStatsEffect<T> : AbstractEffect where T : Effector{


    public abstract float GetAdditionValue(T effector);
    public abstract float GetMultiplicationValue(T effector);


}