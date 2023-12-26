using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameResult result;

    public Text text;
    public Button button;
    void Start()
    {
        int receivedParameter = result.highscore;

        Debug.Log("Received Parameter: " + receivedParameter);
        text.text = receivedParameter.ToString();
        button.onClick.AddListener(()=> SceneManager.LoadScene("MainScene"));
    }
}
