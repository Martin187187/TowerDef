using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTurretsUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject buttonPrefab; // Assign your button prefab in the inspector
    public List<Sprite> spriteList; // Add your sprites in the inspector
    public List<GameObject> turretList; // Add your sprites in the inspector

    public GameObject turret = null;

    public float buttonSize = 50f; // Size of each button
    public float buttonSpacing = 10f; // Spacing between buttons
    public UIStateManager state;

    void Start()
    {
        CreatePanel();
    }

    void CreatePanel()
    {
        // Calculate the total width of the buttons based on size and spacing
        float totalWidth = spriteList.Count * buttonSize + (spriteList.Count - 1) * buttonSpacing;

        // Calculate the starting position for the first button to center them
        float startX = -totalWidth / 2f;

        // Loop through the list of sprites and create buttons for each sprite
        foreach (Sprite sprite in spriteList)
        {
            CreateButton(panel, sprite, startX);
            startX += buttonSize + buttonSpacing; // Update the starting position for the next button
        }
    }

    void CreateButton(GameObject panel, Sprite sprite, float startX)
    {
        // Instantiate a button prefab and set it as a child of the panel
        GameObject button = Instantiate(buttonPrefab, panel.transform);

        // Get the RectTransform of the button to set its position and size
        RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

        // Set the position of the button
        buttonRectTransform.anchoredPosition = new Vector2(startX, 0f);

        // Add an Image component to the button GameObject to display the sprite
        Image imageComponent = button.GetComponent<Image>();
        imageComponent.sprite = sprite;

        // Set the size of the button as needed
        buttonRectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);


        // Add a listener to the button's onClick event
        Button buttonComponent = button.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => OnButtonClick(sprite));
    }
    void OnButtonClick(Sprite clickedSprite)
    {
        Debug.Log("Button clicked! Sprite: " + clickedSprite.name);
        if(State.NONE == state.GetState())
        {
            
            state.SetState(State.TURRET_PLACEMENT);
            turret = turretList[spriteList.IndexOf(clickedSprite)];
        }
    }
}
