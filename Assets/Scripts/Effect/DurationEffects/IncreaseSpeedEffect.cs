using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeedEffect : Effect
{
    public float increaseRatio = 1.2f;
    protected override void EffectParent()
    {
        Projectile projectile = transform.parent.GetComponent<Projectile>();

        if(projectile!=null)
        {
            projectile.speed*=increaseRatio;
        }
    }
}
