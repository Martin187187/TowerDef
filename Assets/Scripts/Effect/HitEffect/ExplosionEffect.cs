
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionEffect : ActionEffect
{
    public float damageRatio = 0.35f;
    public float radius = 1f;

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {
        throw new System.NotImplementedException();
    }

    public override bool OnHitEffect(Effector originEffector, Effector effected)
    {
        return Effect((Projectile)originEffector,(Enemy)effected);
    }
    private bool Effect(Projectile projectile, Enemy enemy){
        
        int damage = (int)(projectile.CalculateDamage() * damageRatio);
        foreach (var enemy2 in EntityManager.Instance.GetEnemies())
        {
            if (enemy2 != null && enemy2 != enemy && projectile is RocketProjectile)
            {
                RocketProjectile rocket = (RocketProjectile)projectile;

                float distance = Vector3.Distance(rocket.impact.position, enemy2.transform.position);

                // Check if this enemy is closer than the current closest
                if (distance < radius)
                {
                    int dmg = (int)(damage * (radius - distance)/radius);
                    enemy2.Damage(dmg);
                }
            }
        }
        return false;
    }

}
