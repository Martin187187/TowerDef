using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    public List<HitEffect> effectList = new List<HitEffect>();
    public Transform target;
    public GameObject projectilePrefab;

    public WorldController controller;
    public float shootingInterval = 3f;
    public float range = 3f;
    public int attack = 10;

    public bool stayOnTarget = false;

    private void Start()
    {
        // Start shooting coroutine
        InvokeRepeating("ShootAtTarget", 0f, shootingInterval);
    }

    private void ShootAtTarget()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) >= range || !stayOnTarget)
        {
            Enemy enemy = Helper.FindNearestEnemy(controller.enemies, new List<Enemy>(), transform.position, range);
            if (enemy != null)
                target = enemy.transform;
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
            projectile.controller = controller;
            projectile.attack = attack;
            // Set the projectile's direction
            projectile.SetTargetDirection(direction);
            
        
            foreach (HitEffect effect in effectList){
                Debug.Log("add");
                projectile.effectList.Add(effect);
            }
        }
    }
}
