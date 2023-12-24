using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    NONE, TURRET_PLACEMENT, TURRET_INSPECTOR
}
public class UIStateManager : MonoBehaviour
{
    
    public static State state = State.NONE;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = State.NONE;
        }
    }
}
