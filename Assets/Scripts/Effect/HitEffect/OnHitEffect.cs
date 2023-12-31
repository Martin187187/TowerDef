using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class OnHitEffect : HitEffect
{
    public float maxLength;
    public int maxHits = 2;
    public override bool Effect(Projectile projectile, Enemy enemy){

        if(projectile.enemyList.Count >= maxHits)
            return false;
        var enemies = EntityManager.Instance.GetEnemies();
        Enemy enemy2 = Helper.FindNearestEnemy(enemies, projectile.enemyList, projectile.transform.position, 5);
        if(enemy2 != null){
            
            Vector3 directionToTarget = enemy2.transform.position - projectile.transform.position;
            projectile.goal = enemy2;
            projectile.SetTargetDirection(directionToTarget);
            return true;
        }
        return false;
    }
}
