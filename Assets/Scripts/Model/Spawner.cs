using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Base goal;

    public WorldController controller;
    public float spawnInterval = 2f;

    public int startingWave = 0;
    public bool startNextWave = false;
    public bool autoplay = false;

    private int enemiesSpawned = 0;
    private bool isRunning = false;

    public Button autoplayButton;
    public Button nextWaveButton;
    public Text waveText;

    public List<HitEffect> possibleEffects = new List<HitEffect>();
    public EffectBagUI bag;

    private void Start()
    {
        autoplayButton.onClick.AddListener(() => OnAutoplayButtonClicked());
        nextWaveButton.onClick.AddListener(() => OnNextWaveButtonClicked());
        autoplayButton.GetComponent<Image>().color = autoplay ? Color.green : Color.red;
        nextWaveButton.GetComponent<Image>().color = startNextWave ? Color.green : Color.red;
        // Start spawning coroutine

    }

    private void OnAutoplayButtonClicked()
    {
        autoplay = !autoplay;
        autoplayButton.GetComponent<Image>().color = autoplay ? Color.green : Color.red;
    }

    private void OnNextWaveButtonClicked()
    {
        startNextWave = !startNextWave;
        nextWaveButton.GetComponent<Image>().color = startNextWave ? Color.green : Color.red;
    }

    void Update()
    {
        if (!isRunning && controller.enemies.Count == 0 && (startNextWave || autoplay))
        {
            enemiesSpawned = 0;
            startNextWave = false;

            nextWaveButton.GetComponent<Image>().color = Color.red;
            startingWave++;
            waveText.text = startingWave.ToString();
            isRunning = true;
            InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
        }
    }
    private void SpawnPrefab()
    {
        // Instantiate the prefab at the current position
        var a = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        Enemy quad = a.GetComponent<Enemy>();
        float moveNoise = Random.Range(-0.2f, 0.2f+0.01f*startingWave);
        quad.moveSpeed += moveNoise;
        quad.hp += (int)(startingWave *Mathf.Log(startingWave+1, 4) * quad.enemyData.health);
        quad.startHp = quad.hp;
        quad.goal = goal;
        quad.controller = controller;
        controller.enemies.Add(quad);
        enemiesSpawned++;

        if (enemiesSpawned > 10 + startingWave * 5)
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