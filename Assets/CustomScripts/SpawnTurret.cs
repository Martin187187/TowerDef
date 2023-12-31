using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Towers;
using Core.Utilities;

public class SpawnTurret : MonoBehaviour
{
    public Tower towerPrefab;
    void Start()
    {
        Tower tower = Instantiate(towerPrefab);
        tower.Initialize(null, IntVector2.zero);
    }

}
