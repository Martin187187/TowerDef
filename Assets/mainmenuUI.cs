using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainmenuUI : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject menus;
    public Button levelSelectButton;
    public Button toMenuButton;

    void Start()
    {
        levelSelectButton.onClick.AddListener(() => { levelSelect.SetActive(true); menus.SetActive(false); });
        toMenuButton.onClick.AddListener(() => { levelSelect.SetActive(false); menus.SetActive(true); });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
