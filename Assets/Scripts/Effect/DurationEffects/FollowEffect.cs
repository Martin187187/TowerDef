using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEffect : Effect
{
    public float maxAngle = 1.0f;
    protected override void EffectParent()
    {
        Projectile projectile = transform.parent.GetComponent<Projectile>();

        if (projectile != null && projectile.goal)
        {
            Debug.Log("test");
            Vector3 currentDirection = projectile.targetDirection;
            Vector3 currentPosition = projectile.transform.position;
            Vector3 target = projectile.goal.transform.position;

            // Calculate the target direction in 2D
            Vector2 targetDirection = (target - currentPosition).normalized;

            // Calculate the angle of the target direction in degrees
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // Calculate the angle of the current direction in degrees
            float currentAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;

            // Constrain the new angle to be within 1 degree of the current angle
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, maxAngle);

            // Convert the new angle back to a Vector2 direction
            Vector2 newDirection = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));

            // Use the newDirection for further calculations or assignments
            // For example, you might want to normalize it if needed:
            newDirection.Normalize();
            projectile.SetTargetDirection(newDirection);

            projectile.targetDirection = newDirection;
        }
    }
}
