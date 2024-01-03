using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ActionGameFramework.Audio;

public abstract class Enemy : Entity
{
    
    protected List<Vector3> path = new List<Vector3>();
    public Base goal;
    protected Vector3 targetPosition;
    
    [SerializeField]
    public float moveSpeed = 2.0f; // Adjust the speed as needed
    public EnemyData enemyData;
    public WorldController controller;
    public Transform rotator;
    public int heatCost = 1;
    public int damage = 1;
    protected Vector2Int oldLocation;
    protected bool firstTime = true;

    void Awake()
    {
        hp = startHp = enemyData.health;
        moveSpeed = enemyData.movementSpeed;
        targetPosition = transform.position;
        Destroy(gameObject, 60);
    }
    void Update()
    {
        Move();
    }

    protected void Move()
    {
        if (goal == null)
            return;
        if (Vector3.Distance(transform.position, goal.transform.position) < 1f)
        {
            goal.Damage(damage);
            SelfDestroy();
            return;
        }
        // Calculate the direction and distance to the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        rotator.transform.rotation = Quaternion.Euler(new Vector3(0f, -angle+90, 0f));
        transform.position += direction * Mathf.Min(GetMovementSpeed() * Time.deltaTime, distanceToTarget);
        
        
        HeatMapRegistration();

        if (transform.position == targetPosition)
        {
            SetNewTargetPosition();
        }
    }

    protected float GetMovementSpeed()
    {
        var slowEffect = gameObject.GetComponentInChildren<SlowEffect>();
        return slowEffect ? moveSpeed * slowEffect.slowRatio : moveSpeed;
    }


    protected void SetNewTargetPosition()
    {
        path = controller.FindPath(transform.position, goal.transform.position);


        if (path != null && path.Count > 0)
        {
            var a = new Vector3(path[0].x, 0, path[0].z);
            if(a == targetPosition)
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
        HeatMapRegistration();
        if (!firstTime)
            controller.heatmap[oldLocation.x, oldLocation.y] -= heatCost;
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            controller.SetMoney(controller.GetMoney() + this.damage*5);
            SelfDestroy();
        }
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
}
