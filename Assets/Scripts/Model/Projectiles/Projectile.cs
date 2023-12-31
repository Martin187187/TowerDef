using UnityEngine;
using System.Collections.Generic;
public class Projectile : MonoBehaviour
{
    public WorldController controller;
    public List<HitEffect> effectList = new List<HitEffect>();

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

            // Apply a 90-degree rotation around the X-axis
            Quaternion xRotation = Quaternion.Euler(90f, 0f, 0f);
            targetRotation *= xRotation;

            // Set the rotation of the projectile
            transform.rotation = targetRotation;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the projectile forward in the target direction
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
        if(explodePrefab)
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
    }

    public void Hit(Enemy enemy)
    {
        if (enemy == null)
        {
            foreach (HitEffect effect in effectList)
                effect.Effect(this, enemy);
            DestroyProjectile();
        }
        else if (!enemyList.Contains(enemy))
        {
            enemyList.Add(enemy);
            bool keepalive = false;
            foreach (HitEffect effect in effectList)
                keepalive |= effect.Effect(this, enemy);
            enemy.Damage(attack);
            if (!keepalive)
                DestroyProjectile();
            else if(explodePrefab)
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
}
