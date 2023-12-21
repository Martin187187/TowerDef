using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DynamicUIBuilder : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentPanel;
    public float buttonWidth = 150f;
    public float buttonHeight = 30f;

    // Sample list of names
    public List<GameObject> names = new List<GameObject>();

    void Start()
    {
        // Create the main panel
        GameObject mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);
        RectTransform mainPanelRect = mainPanel.AddComponent<RectTransform>();
        mainPanelRect.localPosition = Vector3.zero;
        mainPanelRect.sizeDelta = new Vector2(buttonWidth, (buttonHeight+5)*names.Count);
        // Set up vertical layout group for the main panel
        VerticalLayoutGroup verticalLayout = mainPanel.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childForceExpandHeight = false;
        verticalLayout.childControlHeight = false;
        verticalLayout.childControlWidth = false;
        verticalLayout.childForceExpandWidth = false;
        verticalLayout.spacing = 5f; // Adjust as needed

        // Set the size of the buttons in the prefab
        RectTransform buttonPrefabRect = buttonPrefab.GetComponent<RectTransform>();
        buttonPrefabRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);

        // Create buttons dynamically
        foreach (var name in names)
        {
            GameObject button = Instantiate(buttonPrefab, mainPanel.transform);
            button.GetComponentInChildren<Text>().text = name.name;

            // You can add a script here to handle button click events if needed
        }
    }
}
