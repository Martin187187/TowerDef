using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWidth : MonoBehaviour
{
    public float offset = 0;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    

    void Start()
    {
        Initialize();
        AdjustWidth();
    }

    void OnEnable()
    {
        Initialize();
        AdjustWidth();
    }

    void Update()
    {
        AdjustWidth();
    }

    void Initialize()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        // Get the RectTransform component of the parent
        if (transform.parent != null)
        {
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }
    }

    void AdjustWidth()
    {
        // Set the width of the UI element to match the parent's width
        if (rectTransform != null && parentRectTransform != null)
        {
            float newWidth = parentRectTransform.rect.width-offset;
            
            // Only update if the width has changed to avoid unnecessary recalculations
            if (!Mathf.Approximately(rectTransform.sizeDelta.x, newWidth))
            {
                rectTransform.sizeDelta = new Vector2(newWidth * 5, parentRectTransform.rect.height);
            }
        }
    }
}