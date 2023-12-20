using UnityEngine;

public class RocketProjectile : Projectile
{
    
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Enemy"))
    {
        Debug.Log("rocket");
        other.GetComponent<Enemy>().Damage();
        Destroy(gameObject);
    }
}
}
