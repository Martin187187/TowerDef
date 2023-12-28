using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    private static Vector2Int boundary = new Vector2Int(12, 6);

    private static bool checkIfInBoundary(Vector3 position)
    {
        return position.x >= -boundary.x-0.5f && position.x < boundary.x &&
        position.y >= -boundary.y-0.5f && position.y < boundary.y;
    }
    public static Enemy FindNearestEnemy(List<Enemy> enemyList, List<Enemy> ignoreList, Vector3 position, float range)
    {

        float closestDistance = float.MaxValue;
        Enemy nearestEnemy = null;

        // Iterate through the enemies to find the nearest one
        foreach (var enemy in enemyList)
        {
            if (enemy != null && checkIfInBoundary(enemy.transform.position) && !ignoreList.Contains(enemy))
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

    public static Enemy FindNearestToGoalEnemy(List<Enemy> enemyList, List<Enemy> ignoreList, Vector3 position, Vector3 goalPosition, float range)
    {

        float closestDistance = float.MaxValue;
        float closestDistanceSide = float.MaxValue;
        Enemy nearestEnemy = null;

        // Iterate through the enemies to find the nearest one
        foreach (var enemy in enemyList)
        {
            if (enemy != null && checkIfInBoundary(enemy.transform.position) && !ignoreList.Contains(enemy))
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                float distance2 = Vector3.Distance(goalPosition, enemy.transform.position);

                // Check if this enemy is closer than the current closest
                if ((distance2 < closestDistance || (distance2 <= closestDistance && distance < closestDistanceSide)) && distance < range)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        // Set the nearest enemy as the target

        return nearestEnemy;
    }

    public static Enemy FindLowestHpEnemy(List<Enemy> enemyList, List<Enemy> ignoreList, Vector3 position, float range)
    {

        float closestDistanceSide = float.MaxValue;
        int lowestHP = int.MaxValue;
        Enemy lowestHpEnemey = null;

        // Iterate through the enemies to find the nearest one
        foreach (var enemy in enemyList)
        {
            if (enemy != null && checkIfInBoundary(enemy.transform.position) && !ignoreList.Contains(enemy))
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                int hp = enemy.hp;

                // Check if this enemy is closer than the current closest
                if ((hp < lowestHP || (hp <= lowestHP && distance < closestDistanceSide)) && distance < range)
                {
                    lowestHP = hp;
                    lowestHpEnemey = enemy;
                }
            }
        }

        // Set the nearest enemy as the target

        return lowestHpEnemey;
    }

    public static Enemy FindHighestHpEnemy(List<Enemy> enemyList, List<Enemy> ignoreList, Vector3 position, float range)
    {

        float closestDistanceSide = float.MaxValue;
        int lowestHP = int.MinValue;
        Enemy lowestHpEnemey = null;

        // Iterate through the enemies to find the nearest one
        foreach (var enemy in enemyList)
        {
            if (enemy != null && checkIfInBoundary(enemy.transform.position) && !ignoreList.Contains(enemy))
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                int hp = enemy.hp;

                // Check if this enemy is closer than the current closest
                if ((hp > lowestHP || (hp >= lowestHP && distance < closestDistanceSide)) && distance < range)
                {
                    lowestHP = hp;
                    lowestHpEnemey = enemy;
                }
            }
        }

        // Set the nearest enemy as the target

        return lowestHpEnemey;
    }
}