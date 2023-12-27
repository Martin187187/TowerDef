using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

public class EffectBagUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentPanel;
    public float buttonWidth = 150f;
    public float buttonHeight = 30f;
    public List<HitEffect> names = new List<HitEffect>();

    private GameObject mainPanel;
    public GameObject contentPrefab;
    public DynamicUIBuilder builder;

    public UIStateManager state;
    void Start()
    {
        mainPanel = contentPrefab;
        mainPanel.AddComponent<VerticalLayoutGroup>();
        Recalculate();
    }

    public void Recalculate()
    {
        foreach (Transform child in mainPanel.transform)
        {
            // Destroy the child object
            Destroy(child.gameObject);
            // Alternatively, you can use DestroyImmediate(child.gameObject) for immediate removal
        }
        // Create the main panel
        RectTransform mainPanelRect = mainPanel.GetComponent<RectTransform>();
        mainPanelRect.localPosition = new Vector2(0, 0);
        mainPanelRect.sizeDelta = new Vector2(buttonWidth, (buttonHeight + 5) * names.Count);
        // Set the size of the buttons in the prefab

        // Create buttons dynamically
        foreach (var name in names)
        {
            GameObject button = Instantiate(buttonPrefab, mainPanel.transform);
            button.GetComponentInChildren<Text>().text = name.name;

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClick(name));
        }
    }
        
    void OnButtonClick(HitEffect effect)
    {
        if(State.TURRET_INSPECTOR == state.GetState() && !builder.targetTurret.effectList.Contains(effect))
        {
            names.Remove(effect);
            Recalculate();
            builder.targetTurret.effectList.Add(effect);
            builder.Recalculate();
            
        }
    }

}

