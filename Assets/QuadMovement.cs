using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class QuadMovement : MonoBehaviour
{

    private List<Vector3> path = new List<Vector3>();
    public Transform goal;
    private Vector3 targetPosition;
    private float moveSpeed = 2.0f; // Adjust the speed as needed
    public WorldController controller;


    void Start(){
        targetPosition = transform.position;
    }
    void Update()
    {
        if(goal == null)
            return;
        if(Vector3.Distance(transform.position, goal.position) < 1f){
            controller.enemies.Remove(gameObject);
            Destroy(gameObject);
            return;
        }
        // Calculate the direction and distance to the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Move towards the target position using simple vector arithmetic
        transform.position += direction * Mathf.Min(moveSpeed * Time.deltaTime, distanceToTarget);


        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }
    void SetNewTargetPosition()
    {
        path = controller.FindPath(transform.position, goal.position);

        if(path!=null && path.Count>0){
            targetPosition = new Vector3(path[0].x, path[0].y, 0);
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
