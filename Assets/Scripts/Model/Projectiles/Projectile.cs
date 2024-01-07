using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Projectile : Effector
{

    public ParticleSystem fireParticleSystem;
    public List<Enemy> enemyList = new List<Enemy>();
    public Vector3 targetDirection; // New variable to store the target direction
    public Entity goal;
    public float speed = 5f;
    public float lifetime = 3f;
    public int attack = 10;
    public GameObject explodePrefab;

    // Set the target direction when the projectile is instantiated
    public void SetTargetDirection(Vector3 direction)
    {
        targetDirection = direction.normalized;

        if (targetDirection != Vector3.zero)
        {
            // Calculate the rotation to align the forward direction with the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.down);


            // Set the rotation of the projectile
            transform.rotation = targetRotation;
        }
    }

    protected override void Init()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {

        UpdateStaticEffects(this);
        // Move the projectile forward in the target direction
        transform.Translate(targetDirection * CalculateMovementSpeed() * Time.deltaTime, Space.World);
    }


    public void DestroyProjectile()
    {
        Destroy(gameObject);
        if (explodePrefab)
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
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

    public void Hit(Enemy enemy)
    {
        if (enemy == null)
        {
            PerformOnHit(this, enemy, 0);
            DestroyProjectile();
            if (explodePrefab)
                Instantiate(explodePrefab, transform.position, Quaternion.identity);
        }
        else if (!enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
            bool keepalive = PerformOnHit(this, enemy, 0);
            enemy.Damage(CalculateDamage());
            if (!keepalive)
                DestroyProjectile();

            else if (explodePrefab)
                Instantiate(explodePrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            Hit(enemy);
        }
    }


    public float CalculateMovementSpeed()
    {
        float sum = speed;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<ProjectileStatsEffect>().Where((x) => x.modificationStat == ProjectileStatsEffect.ProjectileStats.SPEED))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return sum * mult;
    }

    public int CalculateDamage()
    {
        float sum = attack;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<ProjectileStatsEffect>().Where((x) => x.modificationStat == ProjectileStatsEffect.ProjectileStats.DAMAGE))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return (int)(sum * mult);
    }


}
