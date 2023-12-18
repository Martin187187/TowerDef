using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class QuadMovement : MonoBehaviour
{

    private List<Vector3> path = new List<Vector3>();
    public Transform goal;
    private Vector3 targetPosition;
    public float moveSpeed = 2.0f; // Adjust the speed as needed
    public int hp = 3;
    public WorldController controller;

    private Vector2Int oldLocation;

    private bool firstTime = true;

    void Start()
    {
        targetPosition = transform.position;
    }
    void Update()
    {
        if (goal == null)
            return;
        if (Vector3.Distance(transform.position, goal.position) < 1f)
        {
            SelfDestroy();
            return;
        }
        // Calculate the direction and distance to the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        // Move towards the target position using simple vector arithmetic
        transform.position += direction * Mathf.Min(moveSpeed * Time.deltaTime, distanceToTarget);
        HeatMapRegistration();

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }

    public void SelfDestroy()
    {
        controller.enemies.Remove(gameObject);
        HeatMapRegistration();
        if (!firstTime)
            controller.heatmap[oldLocation.x, oldLocation.y] -= 1;
        Destroy(gameObject);
    }
    private void HeatMapRegistration()
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
    void SetNewTargetPosition()
    {
        path = controller.FindPath(transform.position, goal.position);

        if (path != null && path.Count > 0)
        {
            targetPosition = new Vector3(path[0].x, path[0].y, 0);
        }

    }

    public void Damage()
    {
        hp -= 1;
        if (hp < 0)
            SelfDestroy();
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
