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

            // Calculate the target direction in 3D
            Vector3 targetDirection = (target - currentPosition).normalized;

            // Calculate the rotation quaternion to rotate the current direction towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.forward);

            // Smoothly interpolate the current rotation towards the target rotation
            Quaternion newRotation = Quaternion.RotateTowards(Quaternion.LookRotation(currentDirection, Vector3.forward), targetRotation, maxAngle);

            // Extract the new direction from the new rotation
            Vector3 newDirection = newRotation * Vector3.forward;

            // Use the newDirection for further calculations or assignments
            // For example, you might want to normalize it if needed:
            newDirection.Normalize();

            // Set the new direction for the projectile
            projectile.SetTargetDirection(newDirection);
            projectile.targetDirection = newDirection;
        }
    }
}
