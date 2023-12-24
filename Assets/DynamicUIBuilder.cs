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

    private GameObject mainPanel;
    void Start()
    {
        mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);
    }
    void Recalculate(List<HitEffect> names)
    {
        Destroy(mainPanel);
        
        mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);
        // Create the main panel
        RectTransform mainPanelRect = mainPanel.AddComponent<RectTransform>();
        mainPanelRect.localPosition = Vector3.zero;
        mainPanelRect.sizeDelta = new Vector2(buttonWidth, (buttonHeight + 5) * names.Count);
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

    void Update()
    {
        // Check if the left mouse button is pressed
        if (State.NONE == UIStateManager.state && Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Perform a 2D raycast from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Check if the ray hit something
            if (hit.collider != null)
            {
                // Check if the hit object has the "Turret" tag
                if (hit.collider.CompareTag("Turret"))
                {
                    mainPanel.SetActive(true);
                    UIStateManager.state = State.TURRET_INSPECTOR;
                    Turret turret = hit.collider.GetComponentInChildren<Turret>();
                    Recalculate(turret.effectList);
                }
            }
        }
        else if (State.TURRET_INSPECTOR != UIStateManager.state)
        {
            mainPanel.SetActive(false);
        }
    }
}

