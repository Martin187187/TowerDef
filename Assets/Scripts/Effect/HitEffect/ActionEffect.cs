using System;
using UnityEngine;

public abstract class ActionEffect : AbstractEffect, IComparable{
    
    public int callingLevel;

    public int CompareTo(object obj)
    {
        if (obj == null || !(obj is ActionEffect))
        {
            return 1; // Consider null greater than any non-null object
        }
        return callingLevel.CompareTo(((ActionEffect)obj).callingLevel);
    }

    public abstract bool OnHitEffect(Effector originEffector, Effector effected);
}