using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
public class DoubleProjectile : ActionEffect
{
    public int amountOfExtraProjectiles = 1;
    public float spacing = 0.1f;
    

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {
        
        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        DoubleProjectile otherEffect = (DoubleProjectile)otherObject;
        amountOfExtraProjectiles+=otherEffect.amountOfExtraProjectiles;
        Destroy(otherEffect.gameObject);
    }

    public override bool OnHitEffect(Effector originEffector, Effector effected)
    {
        return Effect((Turret)originEffector, (Projectile)effected);
    }

    private bool Effect(Turret turret, Projectile projectile)
    {
        
        int amountOfProjectiles = amountOfExtraProjectiles + 1;
        for(int i = 1; i < amountOfProjectiles; i++){
            var pos = projectile.transform.position+turret.rotator.transform.rotation * CalculatePosition(i, amountOfExtraProjectiles, spacing);
            var p = turret.InstantiateProjectile(projectile.targetDirection, pos, callingLevel + 1).GetComponent<Projectile>();
            if(p.fireParticleSystem)
            {
                projectile.PlayParticles(projectile.fireParticleSystem, projectile.transform.position, projectile.targetDirection);
            }
        }

        projectile.transform.position+=turret.rotator.transform.rotation *CalculatePosition(0, amountOfExtraProjectiles, spacing);

        return false;
    }

    private Vector3 CalculatePosition(int index, int total, float spacing)
    {
        float start = total*spacing/2;
        return new Vector3(-start + index*spacing, 0, 0);
    }

}
