using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;

    public WorldController controller;
    public float shootingInterval = 3f;
    public float length = 3f;

    private void Start()
    {
        // Start shooting coroutine
        InvokeRepeating("ShootAtTarget", 0f, shootingInterval);
    }

     void FindNearestEnemy()
    {
        if (controller == null || controller.enemies.Count == 0)
        {
            // No enemies or controller is not assigned
            return;
        }

        float closestDistance = float.MaxValue;
        GameObject nearestEnemy = null;

        // Iterate through the enemies to find the nearest one
        foreach (var enemy in controller.enemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                // Check if this enemy is closer than the current closest
                if (distance < closestDistance && distance < length)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        // Set the nearest enemy as the target

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
    }
    private void ShootAtTarget()
    {
        if(target == null || Vector3.Distance(transform.position, target.position) >= length){
            FindNearestEnemy();
        }
        if (target != null)
        {
            // Calculate direction to the target
            Vector3 directionToTarget = target.position - transform.position;

            // Rotate turret to face the target
            transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);

            // Instantiate and launch a projectile towards the target
            InstantiateProjectile(directionToTarget);
        }
    }

    private void InstantiateProjectile(Vector3 direction)
{
    // Instantiate the projectile prefab
    GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    // Configure the projectile component
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    if (projectile != null)
    {
        // Set the projectile's direction
        projectile.SetTargetDirection(direction);
    }
}
}
