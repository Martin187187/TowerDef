using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : Entity
{

    public Spawner spawner;
    public GameResult result;
    void LateUpdate()
    {
        if(hp<0)
        {
            result.highscore = spawner.startingWave;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("GameOverScene");
        }
    }
    public void Damage(int dmg)
    {
        hp-= dmg;
    }

}
