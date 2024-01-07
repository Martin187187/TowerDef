using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TurrestStatsEffect;

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
    [SerializeField]private List<AbstractEffect> effectList = new List<AbstractEffect>();
    [SerializeField]private List<AbstractEffect> spawnableEffects = new List<AbstractEffect>();
    public System.Action OnListChanged;
    [SerializeField]private int money = 600;
    public System.Action OnMoneyChanged;

    public DropZone dropzone;
    public DraggableItem item;
    
    public void SetMoney(int money)
    {
        this.money = money;
        OnMoneyChanged?.Invoke();
    }

    public int GetMoney()
    {
        return this.money;
    }

    public List<AbstractEffect> GetEffects()
    {
        return effectList;
    }

    public void AddEffect(AbstractEffect effect)
    {
        effectList.Add(effect);
        OnListChanged?.Invoke();

    }

    public DraggableItem createItem(AbstractEffect effect, Transform parent)
    {

        DraggableItem a = Instantiate(item);
        a.content = effect;
        a.transform.parent = parent;
        a.transform.localScale = Vector3.one;
        if(effect.sprite)
            a.GetComponent<Image>().overrideSprite = effect.sprite;
        return a;
    }
    public void RemoveEffect(AbstractEffect effect)
    {
        effectList.Remove(effect);
        OnListChanged?.Invoke();
    }

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
    public void createRandomSpawnableEffect()
    {
        Debug.Log("spawn");
        if(spawnableEffects.Count==0)
            return;
        int index = Random.Range(0, spawnableEffects.Count);
        
        GameObject spawnedEffect = Instantiate(spawnableEffects[index].gameObject);
        spawnedEffect.transform.parent = transform;
        var r = spawnedEffect.GetComponent<AbstractEffect>();
        
        AddEffect(r);
    }
    
    private IncreaseTurretStatsEffect createTurretStatsEffect(TurretStats type, float addition, float multiplication)
    {
        GameObject newObject = new GameObject("Effect");
        newObject.transform.parent = transform;

        IncreaseTurretStatsEffect statsEffect = newObject.AddComponent<IncreaseTurretStatsEffect>();
        statsEffect.addition = addition;
        statsEffect.multiplication = multiplication;
        statsEffect.modificationStat = type;
        return statsEffect;
    }

    public void createRandomTurretStatsEffect()
    {
        // Randomly select one of the specific creation methods
        int randomIndex = Random.Range(0, 4);
        switch (randomIndex)
        {
            case 0:
                AddEffect(createRandomAttackTurretStatsEffect());
                break;
            case 1:
                AddEffect(createRandomASTurretStatsEffect());
                break;
            case 2:
                AddEffect(createRandomRSTurretStatsEffect());
                break;
            case 3:
                AddEffect(createRandomRangeTurretStatsEffect());
                break;
            default:
                Debug.LogError("Invalid random index.");
                break;
        }
    }
    IncreaseTurretStatsEffect createRandomAttackTurretStatsEffect()
    {
        float additionValue = Random.Range(0f, 10f);
        float multiplicationValue = Random.Range(1f, 1.05f);
        var r = createTurretStatsEffect(TurretStats.ATTACK_DAMGE, additionValue, multiplicationValue);
        r.name = "Attack +";
        return r;
    }
    IncreaseTurretStatsEffect createRandomASTurretStatsEffect()
    {
        float additionValue = 0;
        float multiplicationValue = Random.Range(0.95f, 1);
        var r = createTurretStatsEffect(TurretStats.ATTACK_INTERVAL, additionValue, multiplicationValue);
        r.name = "Speed +";
        return r;
    }
    IncreaseTurretStatsEffect createRandomRSTurretStatsEffect()
    {
        float additionValue = Random.Range(0f, 20f);
        float multiplicationValue = Random.Range(1f, 1.05f);
        var r = createTurretStatsEffect(TurretStats.ROTATION_SPEED, additionValue, multiplicationValue);
        r.name = "Rotation +";
        return r;
    }
    IncreaseTurretStatsEffect createRandomRangeTurretStatsEffect()
    {
        float additionValue = Random.Range(0f, 2f);
        float multiplicationValue = Random.Range(1f, 1.05f);
        var r = createTurretStatsEffect(TurretStats.RANGE, additionValue, multiplicationValue);
        r.name = "Range +";
        return r;
    }
}