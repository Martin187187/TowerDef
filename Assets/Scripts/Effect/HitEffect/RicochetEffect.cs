using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
public class RicochetEffect : ActionEffect
{
    public int maxExtraHits = 1;
    public float range = 5;

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {
        
        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        RicochetEffect otherEffect = (RicochetEffect)otherObject;
        maxExtraHits+=otherEffect.maxExtraHits;
        Destroy(otherEffect.gameObject);
    }

    public override bool OnHitEffect(Effector originEffector, Effector effected)
    {
        return Effect((Projectile)originEffector, effected);
    }

    private bool Effect(Projectile projectile, Effector effected){

        if(projectile.enemyList.Count > maxExtraHits)
            return false;
        var enemies = EntityManager.Instance.GetEnemies();
        Vector3 target = effected ? effected.transform.position : projectile.transform.position;
        Enemy enemy2 = Helper.FindNearestEnemy(enemies, projectile.enemyList, target, 5);
        if(enemy2 != null){
            
            projectile.transform.position = target;
            Vector3 directionToTarget = enemy2.transform.position - projectile.transform.position;
            projectile.goal = enemy2;
            projectile.SetTargetDirection(directionToTarget);
            if(projectile.fireParticleSystem)
            {
                projectile.PlayParticles(projectile.fireParticleSystem, projectile.transform.position, directionToTarget);
            }
            return true;
        }
        return false;
    }
}
