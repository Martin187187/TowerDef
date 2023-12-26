using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

public class DynamicUIBuilder : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentPanel;
    public float buttonWidth = 150f;
    public float buttonHeight = 30f;

    private GameObject mainPanel;
    public GameObject statsPanel;

    public Text attackText;
    public Text asText;
    public Text ssText;
    public Text rangeText;

    public Button attackButton;
    public Button asButton;
    public Button ssButton;
    public Button rangeButton;
    public Turret targetTurret;

    public Dropdown dropdown;
    public EffectBagUI bag;
    public UIStateManager state;
    void Start()
    {
        mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);

        attackButton.onClick.AddListener(() => OnAttackButton());
        asButton.onClick.AddListener(() => OnASButton());
        ssButton.onClick.AddListener(() => OnSSButton());
        rangeButton.onClick.AddListener(() => OnRangeButton());

        // Subscribe to the OnValueChanged event of the Dropdown
        dropdown.onValueChanged.AddListener((index) => OnDropdownValueChanged(index));
    }

    // Event handler for the Dropdown's value changed
    void OnDropdownValueChanged(int index)
    {
        // Update the selected option based on the index
        targetTurret.strategy = (Turret.TargetStrategy)index;

        // You can do something with the selected option here, like updating other game elements.
        Debug.Log("Selected option: " + targetTurret.strategy);
    }

    private void OnSSButton()
    {
        int cost = targetTurret.CalculateCost();
        int money = targetTurret.controller.GetMoney();
        if (cost <= money)
        {
            targetTurret.controller.SetMoney(money-cost);
            targetTurret.upgraded++;
            targetTurret.shootingSpeed++;
            ssText.text = "" + targetTurret.shootingSpeed;
            setNames();
        }
    }

    private void OnRangeButton()
    {

        int cost = targetTurret.CalculateCost();
        int money = targetTurret.controller.GetMoney();
        if (cost <= money)
        {
            targetTurret.controller.SetMoney(money-cost);
            targetTurret.upgraded++;
            targetTurret.range+=targetTurret.baseRange*0.2f;
            rangeText.text = "" + targetTurret.range;
            setNames();
        }
    }

    private void OnASButton()
    {

        int cost = targetTurret.CalculateCost();
        int money = targetTurret.controller.GetMoney();
        if (cost <= money)
        {
            targetTurret.controller.SetMoney(money-cost);
            targetTurret.upgraded++;
            targetTurret.shootingInterval *= 0.9f;
            asText.text = "" + targetTurret.shootingInterval;
            setNames();
        }
    }

    private void OnAttackButton()
    {

        int cost = targetTurret.CalculateCost();
        int money = targetTurret.controller.GetMoney();
        if (cost <= money)
        {
            targetTurret.controller.SetMoney(money-cost);
            targetTurret.upgraded++;
            targetTurret.attack+=(int)(targetTurret.baseAttack*0.5);
            attackText.text = "" + targetTurret.attack;
            setNames();
        }
    }

    private void setNames()
    {

        int cost = targetTurret.CalculateCost();
        String s = "-" + cost.ToString();
        attackButton.GetComponentInChildren<Text>().text = s;
        asButton.GetComponentInChildren<Text>().text = s;
        ssButton.GetComponentInChildren<Text>().text = s;
        rangeButton.GetComponentInChildren<Text>().text = s;
    }


    public void Recalculate()
    {
        List<HitEffect> names = targetTurret.effectList;
        Destroy(mainPanel);

        mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);
        // Create the main panel
        RectTransform mainPanelRect = mainPanel.AddComponent<RectTransform>();
        mainPanelRect.localPosition = new Vector2(0, -(buttonHeight + 5) / 2 * names.Count - 200);
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

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClick(name));
        }
        setNames();
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (State.NONE == state.GetState() && Input.GetMouseButtonDown(0))
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
                    statsPanel.SetActive(true);
                    state.SetState(State.TURRET_INSPECTOR);
                    Turret turret = hit.collider.GetComponentInChildren<Turret>();
                    targetTurret = turret;
                    Recalculate();
                    attackText.text = "" + turret.attack;
                    asText.text = "" + turret.shootingInterval;
                    ssText.text = "" + turret.shootingSpeed;
                    rangeText.text = "" + turret.range;
                }
            }
        }
        else if (State.TURRET_INSPECTOR != state.GetState())
        {
            mainPanel.SetActive(false);
            statsPanel.SetActive(false);
        }
    }

    void OnButtonClick(HitEffect effect)
    {
        targetTurret.effectList.Remove(effect);
        Recalculate();
        bag.names.Add(effect);
        bag.Recalculate();
    }
}

