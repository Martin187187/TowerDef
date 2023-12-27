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

    // Set the target direction when the projectile is instantiated
    public void SetTargetDirection(Vector3 direction)
    {
        targetDirection = direction.normalized;
        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        }

    }

    private void Start()
    {
        // Automatically destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the projectile forward in the target direction
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if(!enemyList.Contains(enemy)){
                enemyList.Add(enemy);
                bool keepalive = false;
                foreach (HitEffect effect in effectList)
                    keepalive |= effect.Effect(this, enemy);
                enemy.Damage(attack);
                if(!keepalive)
                    Destroy(gameObject);
            }
        }
    }
}
