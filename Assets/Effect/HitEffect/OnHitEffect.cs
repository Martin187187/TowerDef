using UnityEngine;
public class OnHitEffect : HitEffect
{
    public float maxLength;
    public override bool Effect(Projectile projectile, Enemy enemy){

        Enemy enemy2 = Helper.FindNearestEnemy(projectile.controller.enemies, projectile.enemyList, projectile.transform.position, 5);
        if(enemy2 != null){
            
            Vector3 directionToTarget = enemy2.transform.position - projectile.transform.position;
            projectile.SetTargetDirection(directionToTarget);
            return true;
        }
        return false;
    }
}
