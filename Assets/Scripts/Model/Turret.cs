using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    public enum TargetStrategy
    {
        NEAREST_TO_TURRET, NEAREST_TO_GOAL, LOWEST_HP, HIGHEST_HP
    }
    public List<HitEffect> effectList = new List<HitEffect>();
    public List<HitEffect> hardcodeEffectList = new List<HitEffect>();
    public List<GameObject> durationEffectList = new List<GameObject>();
    public Entity target;
    public GameObject projectilePrefab;

    public WorldController controller;
    [SerializeField] public float shootingInterval;
    [SerializeField] public float range;
    [SerializeField] public int attack;
    [SerializeField] public float shootingSpeed;

    public TurretData turretData;
    public int cost = 100;

    public bool stayOnTarget = false;

    public TargetStrategy strategy;
    private float timeSinceLastShot = 0.0f;
    public int upgraded = 0;

    void Awake()
    {

        transform.parent.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        shootingInterval = turretData.shootingInterval;
        range = turretData.attackRange;
        attack = turretData.attackDamage;
        shootingSpeed = turretData.shootingSpeed;
    }
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        // Your shooting logic here
        // Check if enough time has passed and the shots counter is within the limit
        if (timeSinceLastShot >= shootingInterval)
        {
            // Reset the time since the last shot
            timeSinceLastShot = 0.0f;

            ShootAtTarget();
        }
    }

    public int CalculateCost()
    {
        return cost + (int)(upgraded * cost);
    }

    private void ShootAtTarget()
    {
        if (target == null || Vector3.Distance(transform.position, target.transform.position) >= range || !stayOnTarget)
        {
            Enemy enemy;
            switch (strategy)
            {
                case TargetStrategy.NEAREST_TO_GOAL:
                    enemy = Helper.FindNearestToGoalEnemy(controller.enemies, new List<Enemy>(), transform.position, controller.basis.transform.position, range);
                    break;
                case TargetStrategy.LOWEST_HP:
                    enemy = Helper.FindNearestEnemy(controller.enemies, new List<Enemy>(), transform.position, range);
                    break;
                case TargetStrategy.HIGHEST_HP:
                    enemy = Helper.FindHighestHpEnemy(controller.enemies, new List<Enemy>(), transform.position, range);
                    break;
                default:
                    enemy = Helper.FindNearestEnemy(controller.enemies, new List<Enemy>(), transform.position, range);
                    break;
            }
            if (enemy != null)
                target = enemy;
        }
        if (target != null)
        {
            // Calculate direction to the target
            Vector3 directionToTarget = target.transform.position - transform.position;
            if(projectilePrefab.GetComponent<RocketProjectile>() !=null)
                directionToTarget = Vector3.back;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Rotate turret around the Z-axis only
            transform.localRotation = Quaternion.Euler(0f, 0f, -angle+90);

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
            projectile.speed = shootingSpeed;
            projectile.goal = target;
            // Set the projectile's direction
            projectile.SetTargetDirection(direction);


            foreach (HitEffect effect in effectList)
            {
                projectile.effectList.Add(effect);
            }
            foreach (HitEffect effect in hardcodeEffectList)
            {
                projectile.effectList.Add(effect);
            }
            foreach (GameObject effect in durationEffectList)
            {
                GameObject instantiatedEffect = Instantiate(effect, transform.position, Quaternion.identity);
                instantiatedEffect.transform.SetParent(projectile.transform);
            }
        }
    }
}
