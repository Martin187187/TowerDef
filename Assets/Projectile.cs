using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 targetDirection; // New variable to store the target direction

    public float speed = 5f;
    public float lifetime = 3f;

    // Set the target direction when the projectile is instantiated
    public void SetTargetDirection(Vector3 direction)
    {
        targetDirection = direction.normalized;
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
        other.GetComponent<QuadMovement>().Damage();
        Destroy(gameObject);
    }
}
}
