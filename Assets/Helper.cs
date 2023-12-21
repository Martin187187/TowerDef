using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static Enemy FindNearestEnemy(List<Enemy> enemyList, List<Enemy> ignoreList, Vector3 position, float range)
    {

        float closestDistance = float.MaxValue;
        Enemy nearestEnemy = null;

        // Iterate through the enemies to find the nearest one
        foreach (var enemy in enemyList)
        {
            if (enemy != null && !ignoreList.Contains(enemy))
            {
                float distance = Vector3.Distance(position, enemy.transform.position);

                // Check if this enemy is closer than the current closest
                if (distance < closestDistance && distance < range)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        // Set the nearest enemy as the target

        return nearestEnemy;
    }
}