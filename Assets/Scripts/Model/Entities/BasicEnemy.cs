using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void Move()
    {
        if (goal == null)
            return;
        if (Vector3.Distance(transform.position, goal.transform.position) < 1f)
        {
            goal.Damage();
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
        transform.position += direction * Mathf.Min(GetMovementSpeed() * Time.deltaTime, distanceToTarget);
        HeatMapRegistration();

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }
}
