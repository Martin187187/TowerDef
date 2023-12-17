using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;
    public float shootingInterval = 3f;

    private void Start()
    {
        // Start shooting coroutine
        InvokeRepeating("ShootAtTarget", 0f, shootingInterval);
    }

    private void ShootAtTarget()
    {
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
    projectileObject.AddComponent<Rigidbody>().isKinematic = true;
    // Configure the projectile component
    Projectile projectile = projectileObject.AddComponent<Projectile>();
    if (projectile != null)
    {
        // Set the projectile's direction
        projectile.SetTargetDirection(direction);
    }
}
}
