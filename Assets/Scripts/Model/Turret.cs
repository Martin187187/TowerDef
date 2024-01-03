using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ActionGameFramework.Audio;

public class Turret : MonoBehaviour
{
    public enum TargetStrategy
    {
        NEAREST_TO_TURRET, NEAREST_TO_GOAL, LOWEST_HP, HIGHEST_HP
    }
    public List<HitEffect> effectList = new List<HitEffect>();
    public List<HitEffect> hardcodeEffectList = new List<HitEffect>();
    public List<GameObject> durationEffectList = new List<GameObject>();
    public List<Effect> turretDurationEffectList = new List<Effect>();
    public Entity target;
    public GameObject projectilePrefab;
    
    public ParticleSystem fireParticleSystem;
    public RandomAudioSource randomAudioSource;
    public Transform rotator;
    public Transform firingPoint;

    [SerializeField] public float shootingInterval;
    [SerializeField] public float range;
    [SerializeField] public int attack;
    [SerializeField] public float rotationSpeed;
    
    public float angleTolerance = 1f;

    public TurretData turretData;
    public int cost = 100;
    public int baseCost = 100;

    public bool stayOnTarget = false;

    public TargetStrategy strategy;
    private float timer = 0.0f;
    public int upgraded = 0;
    private EntityManager manager;

    private void Start()
    {
        manager = EntityManager.Instance;
        shootingInterval = turretData.shootingInterval;
        range = turretData.attackRange;
        attack = turretData.attackDamage;
        rotationSpeed = turretData.rotationSpeed;
    }
    void Update()
    {

        // Your shooting logic here
        // Check if enough time has passed and the shots counter is within the limit
        if(target==null || !isInRange(target))
        {
            FindTarget();
        }
        if(target!=null)
        {
            AimTarget();
  

            // Check if the timer has reached the desired interval
            if (timer >= shootingInterval && IsFacingTarget())
            {
                Shoot();
                FindTarget();

                // Reset the timer
                timer = 0f;
            }
            timer += Time.deltaTime;
        }
    }
    private void FindTarget()
    {
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
        float distance = Vector3.Distance(target.transform.position, transform.position);
        return distance <= range;
    }
    bool IsFacingTarget()
    {
        if (target != null)
        {
            if(projectilePrefab.GetComponent<RocketProjectile>() !=null)
                return true;
            // Calculate the angle between the rotator's forward direction and the direction to the target
            float angle = Vector3.Angle(rotator.forward, (target.transform.position - rotator.position).normalized);
            
            // Check if the angle is within the tolerance
            return Mathf.Abs(angle) <= angleTolerance;
        }
        else
        {
            Debug.LogWarning("Target is null. Cannot determine if rotator is facing null target.");
            return false;
        }
    }

    private void AimTarget()
    {
        if (target != null)
        {
            // Determine the direction to the target
            Vector3 directionToTarget = target.transform.position - rotator.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            
            if(projectilePrefab.GetComponent<RocketProjectile>() !=null)
                targetRotation.eulerAngles = new Vector3(-30f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            // Use Quaternion.RotateTowards to limit the rotation to a certain angle
            rotator.rotation = Quaternion.RotateTowards(rotator.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public int GetDamage()
    {
        float addition = 0;
        float multiplication = 1;
        foreach (var item in turretDurationEffectList)
        {
            if(item is IncreaseStatsEffect)
            {
                IncreaseStatsEffect increaseStats = (IncreaseStatsEffect)item;
                if(increaseStats.type == IncreaseStatsEffect.Type.ATTACK)
                {
                    if(increaseStats.integration == IncreaseStatsEffect.Integration.ADDITION)
                        addition += increaseStats.amount;
                    else if(increaseStats.integration == IncreaseStatsEffect.Integration.MULTIPLICATION)
                        multiplication += increaseStats.amount;
                }
            }
        }

        return (int)((attack + addition) * multiplication);
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
            InstantiateProjectile(directionToTarget, firingPoint.position);


        }
    }

    private void InstantiateProjectile(Vector3 direction, Vector3 pos)
    {
        // Instantiate the projectile prefab
        GameObject projectileObject = Instantiate(projectilePrefab, pos, Quaternion.identity);
        // Configure the projectile component
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {  
            PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
            randomAudioSource.PlayRandomClip();
            projectile.attack = attack;
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
    public void PlayParticles(ParticleSystem particleSystemToPlay, Vector3 origin, Vector3 lookPosition)
    {
        if (particleSystemToPlay == null)
        {
            return;
        }
        particleSystemToPlay.transform.position = origin;
        particleSystemToPlay.transform.LookAt(lookPosition);
        particleSystemToPlay.Play();
    }

}
