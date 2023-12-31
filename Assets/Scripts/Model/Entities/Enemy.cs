using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Enemy : Entity
{

    protected List<Vector3> path = new List<Vector3>();
    public Base goal;
    protected Vector3 targetPosition;
    [SerializeField]
    public float moveSpeed = 2.0f; // Adjust the speed as needed
    public EnemyData enemyData;
    public WorldController controller;

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

    protected abstract void Move();

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
            targetPosition = new Vector3(path[0].x, 0, path[0].z);

        }

    }


    protected void HeatMapRegistration()
    {

        Vector2Int newLocation = controller.getCellLocation(transform.position) - controller.start;

        if (newLocation != oldLocation)
        {


            if (!firstTime)
                controller.heatmap[oldLocation.x, oldLocation.y] -= 1;
            firstTime = false;
            oldLocation = newLocation;
            controller.heatmap[newLocation.x, newLocation.y] += 1;

        }

    }


    public void SelfDestroy()
    {
        controller.enemiesToRemove.Add(this);
        HeatMapRegistration();
        if (!firstTime)
            controller.heatmap[oldLocation.x, oldLocation.y] -= 1;
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            controller.SetMoney(controller.GetMoney() + 5);
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
