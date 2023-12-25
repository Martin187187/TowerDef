using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : HitEffect
{
    public float damageRatio = 0.35f;
    public float radius = 1f;
    public override bool Effect(Projectile projectile, Enemy enemy){
        
        int damage = (int)(projectile.attack * damageRatio);
        foreach (var enemy2 in projectile.controller.enemies)
        {
            if (enemy2 != null)
            {

                float distance = Vector3.Distance(projectile.transform.position, enemy2.transform.position);

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
