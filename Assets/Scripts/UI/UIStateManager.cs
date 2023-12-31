using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stater
{
    NONE, TURRET_PLACEMENT, TURRET_INSPECTOR
}
public class UIStateManager : MonoBehaviour
{
    
    private Stater state = Stater.NONE;
    private bool changed = true;

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetState(Stater.NONE);
        }
    }



    public void SetState(Stater a)
    {
        state = a;
    }

    public Stater GetState()
    {
        return state;
    }
    
}

