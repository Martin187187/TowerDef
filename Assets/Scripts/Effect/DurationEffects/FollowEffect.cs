using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEffect : Effect
{
    public float maxAngle = 1.0f;
    private Vector3 lastGoal = Vector3.zero;
    
    protected override void EffectParent()
    {
        Projectile projectile = transform.parent.GetComponent<Projectile>();
        
        Vector3 target = projectile.goal ? projectile.goal.transform.position : lastGoal;
        
        if (projectile != null && target != Vector3.zero)
        {
            lastGoal = target;
            
            Vector3 currentDirection = projectile.targetDirection;
            Vector3 currentPosition = projectile.transform.position;

            if(Vector3.Distance(currentPosition, target) < 0.1f || currentPosition.y < 0){
                projectile.Hit(null);
                return;
            }
            // Calculate the target direction in 3D
            Vector3 targetDirection = (target - currentPosition).normalized;

            // Calculate the rotation quaternion to rotate the current direction towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Smoothly interpolate the current rotation towards the target rotation
            Quaternion newRotation = Quaternion.RotateTowards(Quaternion.LookRotation(currentDirection, Vector3.up), targetRotation, maxAngle);

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
