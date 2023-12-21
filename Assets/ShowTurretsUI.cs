using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTurretsU : MonoBehaviour
{
    public List<GameObject> names = new List<GameObject>();
    public GameObject buttonPrefab;

    public int fontSize = 16;
    public float spacing = 10f;

    void Start()
    {
        // Assuming you already have a Canvas and Panel in your scene
        Canvas canvas = GetComponent<Canvas>();
        Transform panelTransform = canvas.transform.Find("Panel");

        if (panelTransform == null)
        {
            Debug.LogError("Panel not found. Make sure you have a Panel in your Canvas.");
            return;
        }

        RectTransform panelRect = panelTransform.GetComponent<RectTransform>();

        // Get the button width and height from the prefab
        RectTransform buttonPrefabRect = buttonPrefab.GetComponent<RectTransform>();
        float buttonWidth = buttonPrefabRect.sizeDelta.x;
        float buttonHeight = buttonPrefabRect.sizeDelta.y;

        // Calculate the total width of the buttons and spacing
        float totalWidth = (buttonWidth + spacing) * names.Count - spacing;

        // Calculate the starting X position so that buttons grow from the middle
        float startX = -totalWidth / 2f + buttonWidth / 2f;

        // Create Buttons for each name
        for (int i = 0; i < names.Count; i++)
        {
            // Calculate position based on index
            float xPos = startX + (buttonWidth + spacing) * i;

            // Create Button
            GameObject buttonObj = Instantiate(buttonPrefab, panelTransform);
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
            buttonRect.anchoredPosition = new Vector2(xPos, 0);

            // Set Button Text
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = names[i].name;
            buttonText.fontSize = fontSize;

            // Add Click Event
            int currentIndex = i; // Store the current index in a variable
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => ButtonClick(names[currentIndex].name));
        }
    }

    void ButtonClick(string name)
    {
        Debug.Log("Button clicked for " + name);
    }
}
