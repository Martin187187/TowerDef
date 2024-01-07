using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class PenetrateEffect : ActionEffect
{

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {
        
        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        PenetrateEffect otherEffect = (PenetrateEffect)otherObject;
        Destroy(otherEffect.gameObject);
    }

    public override bool OnHitEffect(Effector originEffector, Effector effected)
    {
        return true;
    }

}
