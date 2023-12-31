using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject prefabToSpawnBoss;
    public Base goal;

    public WorldController controller;
    public float spawnInterval = 2f;

    public int startingWave = 0;
    public bool startNextWave = false;
    public bool autoplay = false;

    public int enemiesSpawned = 0;
    public bool isRunning = false;


    public List<HitEffect> possibleEffects = new List<HitEffect>();
    public EffectBagUI bag;
    private EntityManager manager;

    private void Start()
    {
        manager = EntityManager.Instance;

    }


    void Update()
    {
        if (!isRunning && manager.GetEnemies().Count == 0 && (startNextWave || autoplay))
        {
            enemiesSpawned = 0;
            startNextWave = false;

            
            startingWave++;
            isRunning = true;
            InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
        }
    }

    public int GetWaveSize()
    {
        return 10 + startingWave * 5;
    }
    private void SpawnPrefab()
    {
        // Instantiate the prefab at the current position
        GameObject a;
        if(enemiesSpawned % 50 == 49)
            a = Instantiate(prefabToSpawnBoss, transform.position, Quaternion.identity);
        else
            a = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        a.transform.SetParent(manager.getEnemiesTransform());
        Enemy quad = a.GetComponent<Enemy>();
        float moveNoise = Random.Range(-0.2f, 0.2f+0.01f*startingWave);
        quad.moveSpeed += moveNoise;
        quad.hp += (int)(startingWave *Mathf.Log(startingWave+1, 4) * quad.enemyData.health);
        quad.startHp = quad.hp;
        quad.goal = goal;
        quad.controller = controller;
        enemiesSpawned++;

        if (enemiesSpawned > GetWaveSize())
        {
            isRunning = false;
            CancelInvoke("SpawnPrefab");

            if (startingWave % 5 == 1 && possibleEffects.Count > 0)
            {
                // Generate a random index within the valid range
                int randomIndex = Random.Range(0, possibleEffects.Count);

                // Return the element at the random index
                HitEffect effect = possibleEffects[randomIndex];
                bag.names.Add(effect);
                bag.Recalculate();
            }
        }
    }

}