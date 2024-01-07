using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Enemy : Entity
{

    protected List<Vector3> path = new List<Vector3>();
    public Base goal;
    protected Vector3 targetPosition;
    
    private int damage = 1;
    private float moveSpeed = 2.0f;
    public EnemyData enemyData;
    public WorldController controller;
    public Transform rotator;
    public int heatCost = 1;
    protected Vector2Int oldLocation;
    protected bool firstTime = true;

    EntityManager entityManager;

    protected override void Init()
    {
        entityManager = EntityManager.Instance;
        hp = startHp = enemyData.health;
        moveSpeed = enemyData.movementSpeed;
        targetPosition = transform.position;
        Destroy(gameObject, 60);
    }
    void Update()
    {
        
        UpdateStaticEffects(this);
        Move();
    }

    protected void Move()
    {
        if (goal == null)
            return;
        if (Vector3.Distance(transform.position, goal.transform.position) < 1f)
        {
            goal.Damage(CalculateDamage());
            SelfDestroy();
            return;
        }
        // Calculate the direction and distance to the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);


        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        rotator.transform.rotation = Quaternion.Euler(new Vector3(0f, -angle + 90, 0f));
        transform.position += direction * Mathf.Min(CalculateMovementSpeed() * Time.deltaTime, distanceToTarget);


        HeatMapRegistration();

        if (transform.position == targetPosition)
        {
            SetNewTargetPosition();
        }
    }


    protected void SetNewTargetPosition()
    {
        path = controller.FindPath(transform.position, goal.transform.position);


        if (path != null && path.Count > 0)
        {
            var a = new Vector3(path[0].x, 0, path[0].z);
            if (a == targetPosition)
                path.RemoveAt(0);
            if (path != null && path.Count > 0)
                targetPosition = new Vector3(path[0].x, 0, path[0].z);
        }

    }


    protected void HeatMapRegistration()
    {

        Vector2Int newLocation = controller.getCellLocation(transform.position) - controller.start;

        if (newLocation != oldLocation)
        {


            if (!firstTime)
                controller.heatmap[oldLocation.x, oldLocation.y] -= heatCost;
            firstTime = false;
            oldLocation = newLocation;
            controller.heatmap[newLocation.x, newLocation.y] += heatCost;

        }

    }


    public void SelfDestroy()
    {
        if(Random.Range(0, 50) < damage)
        {
            entityManager.createRandomTurretStatsEffect();
        }
        HeatMapRegistration();
        if (!firstTime)
            controller.heatmap[oldLocation.x, oldLocation.y] -= heatCost;
        Destroy(gameObject);
    }

    public bool Damage(int damage)
    {
       hp-=damage;
        if (hp < 0)
        {
            entityManager.SetMoney(entityManager.GetMoney() + this.heatCost * 5);
            SelfDestroy();
            return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        DrawPath();
    }

    void DrawPath()
    {
        if (path == null || path.Count < 2)
        {
            // Not enough points to form a path
            return;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.blue);
        }
    }

    public float CalculateMovementSpeed()
    {
        float sum = moveSpeed;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<EnemyStatsEffect>().Where((x) => x.modificationStat == EnemyStatsEffect.EnemyStats.MOVEMENT_SPEED))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return sum * mult;
    }
    
    public int CalculateDamage()
    {
        float sum = damage;
        float mult = 1;
        foreach (var item in GetEffectList().OfType<EnemyStatsEffect>().Where((x) => x.modificationStat == EnemyStatsEffect.EnemyStats.DAMAGE))
        {
            sum += item.GetAdditionValue(this);
            mult *= item.GetMultiplicationValue(this);
        }

        return (int)(sum * mult);
    }

    public void IncreaseBaseMovementSpeed(float value)
    {
        moveSpeed += value;
    }
    
}

