using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    NONE, TURRET_PLACEMENT, TURRET_INSPECTOR
}
public class UIStateManager : MonoBehaviour
{
    
    private State state = State.NONE;
    public Text stateText;
    public Button button;


    void Start()
    {
        
        button.onClick.AddListener(() => SetState(State.NONE));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetState(State.NONE);
        }
    }

    public void SetState(State a)
    {
        state = a;
        stateText.text = a.ToString();
    }

    public State GetState()
    {
        return state;
    }
    
}

