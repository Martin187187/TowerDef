using UnityEngine;
public class OnHitEffect : HitEffect
{
    public float maxLength;
    public int maxHits = 2;
    public override bool Effect(Projectile projectile, Enemy enemy){

        if(projectile.enemyList.Count >= maxHits)
            return false;
        Enemy enemy2 = Helper.FindNearestEnemy(projectile.controller.enemies, projectile.enemyList, projectile.transform.position, 5);
        if(enemy2 != null){
            
            Vector3 directionToTarget = enemy2.transform.position - projectile.transform.position;
            projectile.goal = enemy2;
            projectile.SetTargetDirection(directionToTarget);
            return true;
        }
        return false;
    }
}
