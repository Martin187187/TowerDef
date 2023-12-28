using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeView : MonoBehaviour
{
    Camera mainCamera;
    public Button button;
    private bool set = false;
    void Start()
    {
        mainCamera = Camera.main;
        button.onClick.AddListener(ChangeCamera);
    }
    
    private void ChangeCamera()
    {
        mainCamera.orthographic = set;
        set = !set;
    }
}
