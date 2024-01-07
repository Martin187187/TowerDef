using UnityEngine;
using UnityEngine.UI;

public class EffectListUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;
    public int numberOfButtons = 10;
    public float buttonHeight = 50f;
    public float spacing = 5f;

    void Start()
    {
        CreateButtons();
    }

    void CreateButtons()
    {
        RectTransform contentTransform = GetComponent<RectTransform>();

        for (int i = 0; i < numberOfButtons; i++)
        {
            // Instantiate button prefab
            GameObject button = Instantiate(buttonPrefab, contentTransform);
            
            // Set button text (optional)
            button.GetComponentInChildren<Text>().text = "Button " + (i + 1);

            // Set button position and size
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(0, -i * (buttonHeight + spacing));
            buttonTransform.sizeDelta = new Vector2(contentTransform.rect.width, buttonHeight);
            button.transform.parent = content;
        }

        RectTransform contentTransform2 = content.GetComponent<RectTransform>();
        // Adjust content size based on the number of buttons
        float contentHeight = numberOfButtons * (buttonHeight + spacing) - spacing;
        contentTransform2.sizeDelta = new Vector2(contentTransform2.sizeDelta.x, contentHeight);
    }
}
