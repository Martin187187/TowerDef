using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ActionGameFramework.Audio;
using Unity.VisualScripting;

public class Turret : Effector
{
    public enum TargetStrategy
    {
        NEAREST_TO_TURRET, NEAREST_TO_GOAL, LOWEST_HP, HIGHEST_HP
    }

    [HideInInspector] public Entity target;
    public GameObject projectilePrefab;

    public ParticleSystem fireParticleSystem;
    public RandomAudioSource randomAudioSource;
    public Transform rotator;
    public Transform firingPoint;


    private float shootingInterval;
    private float range;
    private int attack;
    private float rotationSpeed;
    [HideInInspector] public float neededCooldown = 0.5f;
     public int cost = 100;
     public int baseCost = 100;

    public float angleTolerance = 1f;

    public TurretData turretData;

    public bool stayOnTarget = false;

    [HideInInspector] public TargetStrategy strategy;
    private float timer = 0.0f;
    private float cooldownTimer = 0.0f;
    [HideInInspector] public int upgraded = 0;

    [HideInInspector] public int kills;


    protected override void Init()
    {
        shootingInterval = turretData.shootingInterval;
        range = turretData.attackRange;
        attack = turretData.attackDamage;
        rotationSpeed = turretData.rotationSpeed;
        cost = turretData.upgradeCost;
        baseCost = turretData.turrestCost;
        neededCooldown = turretData.cooldownTime;
    }
    void Update()
    {

        if(cooldownTimer < neededCooldown){
            cooldownTimer += Time.deltaTime;
            return;
        }
        UpdateStaticEffects(this);
        if (target == null || !isInRange(target))
        {
            FindTarget();
        }
        if (target != null)
        {
            AimTarget();


            // Check if the timer has reached the desired interval
            if (timer >= CalculateAttackInterval() && IsFacingTarget())
            {
                Shoot();
                cooldownTimer = 0;
                FindTarget();

                // Reset the timer
                timer = 0f;
            }
            timer += Time.deltaTime;
        }
    }
    private void FindTarget()
    {
        float range = CalculateRange();
        if (target == null || Vector3.Distance(transform.position, target.transform.position) >= range || !stayOnTarget)
        {
            Enemy enemy;
            List<Enemy> enemies = manager.GetEnemies();
            switch (strategy)
            {
                case TargetStrategy.NEAREST_TO_GOAL:
                    enemy = Helper.FindNearestToGoalEnemy(enemies, new List<Enemy>(), transform.position, manager.getBasisTransform().position, range);
                    break;
                case TargetStrategy.LOWEST_HP:
                    enemy = Helper.FindNearestEnemy(enemies, new List<Enemy>(), transform.position, range);
                    break;
                case TargetStrategy.HIGHEST_HP:
                    enemy = Helper.FindHighestHpEnemy(enemies, new List<Enemy>(), transform.position, range);
                    break;
                default:
                    enemy = Helper.FindNearestEnemy(enemies, new List<Enemy>(), transform.position, range);
                    break;
            }
            target = enemy;
        }
    }

    bool isInRange(Entity target)
    {
        float range = CalculateRange();
        float distance = Vector3.Distance(target.transform.position, transform.position);
        return distance <= range;
    }
    bool IsFacingTarget()
    {
        if (target != null)
        {
            if (projectilePrefab.GetComponent<RocketProjectile>() != null)
                return true;
            // Calculate the angle between the rotator's forward direction and the direction to the target
            float angle = Vector3.Angle(rotator.forward, (target.transform.position - CalculatePosition()).normalized);

            // Check if the angle is within the tolerance
            return Mathf.Abs(angle) <= angleTolerance;
        }
        else
        {
            Debug.LogWarning("Target is null. Cannot determine if rotator is facing null target.");
            return false;
        }
    }

    public Vector3 CalculatePosition()
    {
        return new Vector3(rotator.position.x, firingPoint.position.y, rotator.position.z);
    }
    private void AimTarget()
    {
        if (target != null)
        {
            // Determine the direction to the target
            
            Vector3 directionToTarget = target.transform.position - CalculatePosition();
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            if (projectilePrefab.GetComponent<RocketProjectile>() != null)
                targetRotation.eulerAngles = new Vector3(-30f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            // Use Quaternion.RotateTowards to limit the rotation to a certain angle
            rotator.rotation = Quaternion.RotateTowards(rotator.rotation, targetRotation, CalculateRotationSpeed() * Time.deltaTime);
        }
    }

    public int CalculateCost()
    {
        return cost + (int)(upgraded * cost);
    }

    private void Shoot()
    {

        if (target != null)
        {
            // Calculate direction to the target


            Vector3 directionToTarget = rotator.forward;


            // Instantiate and launch a projectile towards the target
            InstantiateProjectile(directionToTarget, firingPoint.position, 0);


        }
    }

    public GameObject InstantiateProjectile(Vector3 direction, Vector3 pos, int callingLevel)
    {
        // Instantiate the projectile prefab
        GameObject projectileObject = Instantiate(projectilePrefab, pos, Quaternion.identity);
        // Configure the projectile component
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            PlayParticles(fireParticleSystem, pos, direction);
            randomAudioSource.PlayRandomClip();
            projectile.attack = CalculateAttackDamage();
            projectile.goal = target;
            // Set the projectile's direction
            projectile.SetTargetDirection(direction);

            // TODO: ONHIT
            PerformOnHit(this, projectile, callingLevel);
        }
        return projectileObject;
    }

    
    public void PlayParticles(ParticleSystem particleSystemToPlay, Vector3 origin, Vector3 direction)
    {
        if (particleSystemToPlay == null)
        {
            return;
        }
        particleSystemToPlay.transform.position = origin;
        particleSystemToPlay.transform.rotation = Quaternion.LookRotation(direction, Vector3.down);
        particleSystemToPlay.Play();
    }



    public int CalculateAttackDamage()
    {
        float sum = attack;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<TurrestStatsEffect>().Where((x) => x.modificationStat == TurrestStatsEffect.TurretStats.ATTACK_DAMGE))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return (int)(sum * mult);
    }

    public float CalculateAttackInterval()
    {
        float sum = shootingInterval;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<TurrestStatsEffect>().Where((x) => x.modificationStat == TurrestStatsEffect.TurretStats.ATTACK_INTERVAL))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return sum * mult;
    }

    public float CalculateRotationSpeed()
    {
        float sum = rotationSpeed;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<TurrestStatsEffect>().Where((x) => x.modificationStat == TurrestStatsEffect.TurretStats.ROTATION_SPEED))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return sum * mult;
    }

    public float CalculateRange()
    {
        float sum = range;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<TurrestStatsEffect>().Where((x) => x.modificationStat == TurrestStatsEffect.TurretStats.RANGE))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return sum * mult;
    }

    public void IncreaseBaseAttackDamage(int value)
    {
        attack += value;
    }
    public void MultiplyBaseAttackInterval(float value)
    {
        shootingInterval += value;
    }
    public void IncreaseBaseRotationSpeed(float value)
    {
        rotationSpeed += value;
    }
    public void IncreaseBaseRange(float value)
    {
        range += value;
    }
}
