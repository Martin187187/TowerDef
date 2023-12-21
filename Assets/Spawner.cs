using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform goal;

    public WorldController controller;
    public float spawnInterval = 2f;

    private void Start()
    {
        // Start spawning coroutine
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
    }

    private void SpawnPrefab()
    {
        // Instantiate the prefab at the current position
        var a=Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        Enemy quad = a.GetComponent<Enemy>();
        quad.goal = goal;
        quad.controller = controller;
        controller.enemies.Add(quad);
    }
}