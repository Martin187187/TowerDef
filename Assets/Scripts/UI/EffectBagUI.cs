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

    private GameObject mainPanel;
    public GameObject contentPrefab;
    public DynamicUIBuilder builder;
    public EntityManager entityManager;

    public UIStateManager state;
    void Start()
    {

        entityManager = EntityManager.Instance;
        entityManager.OnListChanged += Recalculate;
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
        mainPanelRect.sizeDelta = new Vector2(buttonWidth, (buttonHeight + 5) * entityManager.GetEffects().Count);
        // Set the size of the buttons in the prefab

        // Create buttons dynamically
        foreach (var name in entityManager.GetEffects())
        {
            GameObject button = Instantiate(buttonPrefab, mainPanel.transform);
            button.GetComponentInChildren<Text>().text = name.name;

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClick(name));
        }
    }

    void OnButtonClick(AbstractEffect effect)
    {
        if (Stater.TURRET_INSPECTOR == state.GetState())
        {
            AbstractEffect compatibleEffect = builder.targetTurret.GetCompatibleEffect(effect);

            if (compatibleEffect)
            {
                entityManager.RemoveEffect(effect);
                compatibleEffect.ConsumeOtherObject(effect);
                Recalculate();
                builder.Recalculate();
                return;
            }

            entityManager.RemoveEffect(effect);
            builder.targetTurret.AddEffect(effect);
            Recalculate();
            builder.Recalculate();

        }
    }

}

