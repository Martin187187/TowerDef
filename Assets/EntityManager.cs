using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    private static EntityManager instance;
    public static EntityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EntityManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("EntityManager");
                    instance = obj.AddComponent<EntityManager>();
                }
            }
            return instance;
        }
    }

    protected Transform enemiesParent;
    protected Transform turretsParent;
    protected Transform basis;

    private void Awake()
    {
        // Ensure there's only one instance of the EnemyManager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        enemiesParent = GameObject.Find("Enemies").transform;
        turretsParent = GameObject.Find("Turrets").transform;
        basis = GameObject.Find("Base").transform;

    }
    public Transform getBasisTransform()
    {
        return basis;
    }
    public Transform getEnemiesTransform()
    {
        return enemiesParent;
    }
    public Transform getTurretsTransform()
    {
        return turretsParent;
    }
    public List<Enemy> GetEnemies()
    {
        return new List<Enemy>(enemiesParent.GetComponentsInChildren<Enemy>());
    }

    public List<Turret> GetTurrets()
    {
        return new List<Turret>(turretsParent.GetComponentsInChildren<Turret>());
    }
}