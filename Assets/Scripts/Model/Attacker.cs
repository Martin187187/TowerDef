using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGameFramework.Audio;

public class Attacker : MonoBehaviour
{
    public ParticleSystem fireParticleSystem;
    public RandomAudioSource randomAudioSource;
    public Transform rotator;
    public Transform firingPoint;
    public GameObject projectilePrefab;

    public float rotationSpeed = 120f;
    public float angleTolerance = 1f;
    public float shootingInterval = 2f; // Set the interval in seconds
    public float range = 5f;
    
    private Target target;
    private float timer = 0f;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if(target==null || !isInRange(target))
        {
            FindTarget();
        }
        if(target!=null)
        {
            AimTarget();
  

            // Check if the timer has reached the desired interval
            if (timer >= shootingInterval && IsFacingTarget())
            {
                Shoot();
                FindTarget();

                // Reset the timer
                timer = 0f;
            }
            timer += Time.deltaTime;
        }

    }

    
    public void FindTarget()
    {
        Target[] targetScripts = FindObjectsOfType<Target>();
        float closest = float.MaxValue;
        Target closestTarget = null;
        foreach (Target targetScript in targetScripts)
        {
            float distance = Vector3.Distance(targetScript.transform.position, transform.position);
            if(distance < closest && distance <= range)
            {
                closest = distance;
                closestTarget = targetScript;
            }
        }

        if(closestTarget)
        {
            target = closestTarget;
        }
    }
    bool isInRange(Target target)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        return distance <= range;
    }
    void AimTarget()
    {
        // Check if the target is not null
        if (target != null)
        {
            // Determine the direction to the target
            Vector3 directionToTarget = target.transform.position - rotator.position;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Use Quaternion.RotateTowards to limit the rotation to a certain angle
            rotator.rotation = Quaternion.RotateTowards(rotator.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target is null. Cannot rotate towards null target.");
        }
    }
    bool IsFacingTarget()
    {
        if (target != null)
        {
            // Calculate the angle between the rotator's forward direction and the direction to the target
            float angle = Vector3.Angle(rotator.forward, (target.transform.position - rotator.position).normalized);

            // Check if the angle is within the tolerance
            return Mathf.Abs(angle) <= angleTolerance;
        }
        else
        {
            Debug.LogWarning("Target is null. Cannot determine if rotator is facing null target.");
            return false;
        }
    }
    public void Shoot()
    {

        PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
        randomAudioSource.PlayRandomClip();
        InstantiateProjectile();
    }
    private void InstantiateProjectile()
    {
        Vector3 directionToTarget = target.transform.position - firingPoint.position;
        // Instantiate the projectile prefab
        GameObject projectileObject = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        // Configure the projectile component
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.SetTargetDirection(directionToTarget);

    }
    public void PlayParticles(ParticleSystem particleSystemToPlay, Vector3 origin, Vector3 lookPosition)
    {
        if (particleSystemToPlay == null)
        {
            return;
        }
        particleSystemToPlay.transform.position = origin;
        particleSystemToPlay.transform.LookAt(lookPosition);
        particleSystemToPlay.Play();
    }

}
