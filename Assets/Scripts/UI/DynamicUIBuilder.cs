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

    public Button closeButton;
    public Button attackButton;
    public Button asButton;
    public Button ssButton;
    public Button rangeButton;
    public Turret targetTurret;

    public Dropdown dropdown;
    public EffectBagUI bag;
    public UIStateManager state;

    public WorldController controller;

    public EntityManager manager;
    void Start()
    {
        manager = EntityManager.Instance;
        mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);

        attackButton.onClick.AddListener(() => OnAttackButton());
        asButton.onClick.AddListener(() => OnASButton());
        ssButton.onClick.AddListener(() => OnSSButton());
        rangeButton.onClick.AddListener(() => OnRangeButton());
        closeButton.onClick.AddListener(() => state.SetState(Stater.NONE));

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
        int money = manager.GetMoney();
        if (cost <= money)
        {
            manager.SetMoney(money - cost);
            targetTurret.upgraded++;
            targetTurret.IncreaseBaseRotationSpeed(targetTurret.turretData.rotationSpeed * 0.1f);
            ssText.text = "" + targetTurret.CalculateRotationSpeed();
            setNames();
        }
    }

    private void OnRangeButton()
    {

        int cost = targetTurret.CalculateCost();
        int money = manager.GetMoney();
        if (cost <= money)
        {
            manager.SetMoney(money - cost);
            targetTurret.upgraded++;
            targetTurret.IncreaseBaseRange(targetTurret.turretData.attackRange * 0.2f);
            rangeText.text = "" + targetTurret.CalculateRange();
            setNames();
        }
    }

    private void OnASButton()
    {

        int cost = targetTurret.CalculateCost();
        int money = manager.GetMoney();
        if (cost <= money)
        {
            manager.SetMoney(money - cost);
            targetTurret.upgraded++;
            targetTurret.MultiplyBaseAttackInterval(0.95f);
            asText.text = "" + targetTurret.CalculateAttackInterval();
            setNames();
        }
    }

    private void OnAttackButton()
    {

        int cost = targetTurret.CalculateCost();
        int money = manager.GetMoney();
        if (cost <= money)
        {
            manager.SetMoney(money - cost);
            targetTurret.upgraded++;
            targetTurret.IncreaseBaseAttackDamage((int)(targetTurret.turretData.attackDamage * 0.5));
            attackText.text = "" + targetTurret.CalculateAttackDamage();
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
        List<AbstractEffect> names = targetTurret.GetEffectList();
        Destroy(mainPanel);

        mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(parentPanel, false);
        // Create the main panel
        RectTransform mainPanelRect = mainPanel.AddComponent<RectTransform>();
        mainPanelRect.localPosition = new Vector2(-180 + 26.84f, -(buttonHeight + 5) / 2 * names.Count - 380);
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

        attackText.text = "" + targetTurret.CalculateAttackDamage();
        asText.text = "" + targetTurret.CalculateAttackInterval();
        ssText.text = "" + targetTurret.CalculateRotationSpeed();
        rangeText.text = "" + targetTurret.CalculateRange();
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Stater.NONE == state.GetState() && Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Perform a raycast from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // Check if the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has the "Turret" tag
                if (hit.collider.CompareTag("Turret"))
                {
                    Debug.Log("dhn");
                    mainPanel.SetActive(true);
                    statsPanel.SetActive(true);
                    state.SetState(Stater.TURRET_INSPECTOR);

                    // Assuming Turret script is attached to the same GameObject as the collider
                    Turret turret = hit.collider.GetComponentInChildren<Turret>();

                    // If Turret script is on a child GameObject, you might need to use GetComponentInChildren
                    // Turret turret = hit.collider.GetComponentInChildren<Turret>();

                    targetTurret = turret;
                    Recalculate();
                    attackText.text = "" + turret.CalculateAttackDamage();
                    asText.text = "" + turret.CalculateAttackInterval();
                    ssText.text = "" + turret.CalculateRotationSpeed();
                    rangeText.text = "" + turret.CalculateRange();
                    dropdown.value = (int)targetTurret.strategy;
                }
            }
        }
        else if (Stater.TURRET_INSPECTOR != state.GetState())
        {
            mainPanel.SetActive(false);
            statsPanel.SetActive(false);
        }
    }

    void OnButtonClick(AbstractEffect effect)
    {
        targetTurret.RemoveEffect(effect);
        manager.AddEffect(effect);
        Recalculate();
        bag.Recalculate();
    }
}

