using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "ScriptableObjects/TurretData")]
public class TurretData : ScriptableObject
{
    public int attackDamage;
    public float shootingInterval;
    public float rotationSpeed;
    public float attackRange;
    
}
