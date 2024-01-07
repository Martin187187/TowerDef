using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUiScript : MonoBehaviour
{

    public Spawner spawner;
    public GameObject wave;
    public Button button;
    public Button auto;
    public Text money;
    public WorldController controller;
    public EntityManager manager;
    void Start()
    {
        manager = EntityManager.Instance;
        manager.OnMoneyChanged += UpdateMoney;
        spawner.OnRunningChange += UpdateRunning;
        button.onClick.AddListener(StartNextWave);
        auto.onClick.AddListener(Auto);
    }

    // Update is called once per frame
    void UpdateRunning()
    {
        wave.SetActive(!spawner.IsRunning);
    }

    void UpdateMoney()
    {
        money.text = manager.GetMoney().ToString();
    }

    public void StartNextWave()
    {
        spawner.startNextWave = true;
    }
    public void Auto()
    {
        spawner.autoplay = !spawner.autoplay;
        auto.image.color = spawner.autoplay ? Color.green : auto.colors.normalColor;
        
    }

}
