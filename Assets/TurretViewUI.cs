using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurretViewUI : MonoBehaviour
{
    private EntityManager manager;
    public UIStateManager state;
    public GameObject mainPanel;
    public GameObject bag;


    public Turret targetTurret;
    // ui interactives

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI asText;
    public TextMeshProUGUI ssText;
    public TextMeshProUGUI rangeText;
    public Button attackButton;
    public Button asButton;
    public Button ssButton;
    public Button rangeButton;

    public Button closeButton;

    public TMP_Dropdown dropdown;

    public DropZone zone;

    public DropZone zoneBag;


    void Start()
    {
        manager = EntityManager.Instance;
        manager.OnListChanged += Recalculate;
        attackButton.onClick.AddListener(() => OnAttackButton());
        asButton.onClick.AddListener(() => OnASButton());
        ssButton.onClick.AddListener(() => OnSSButton());
        rangeButton.onClick.AddListener(() => OnRangeButton());
        closeButton.onClick.AddListener(() => state.SetState(Stater.NONE));

        // Subscribe to the OnValueChanged event of the Dropdown
        dropdown.onValueChanged.AddListener((index) => OnDropdownValueChanged(index));
    }
    public void Recalculate()
    {
        if(!targetTurret)
            return;

        attackText.text = "" + targetTurret.CalculateAttackDamage();
        asText.text = "" + targetTurret.CalculateAttackInterval();
        ssText.text = "" + targetTurret.CalculateRotationSpeed();
        rangeText.text = "" + targetTurret.CalculateRange();

        zone.ClearChilds(targetTurret.GetEffectList());
        
        zoneBag.ClearChilds(manager.GetEffects());
    }

    // Update is called once per frame
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
                    state.SetState(Stater.TURRET_INSPECTOR);
                    mainPanel.SetActive(true);
                    bag.SetActive(true);
                    // Assuming Turret script is attached to the same GameObject as the collider
                    targetTurret = hit.collider.GetComponentInChildren<Turret>();
                    Recalculate();

                }
            }
        }
        else if (Stater.TURRET_INSPECTOR != state.GetState())
        {
            mainPanel.SetActive(false);
            bag.SetActive(false);
        }
    }



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
        }
    }
}
