using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    public int damage = 1;
    protected override void EffectParent()
    {
        // Check if the GameObject has a parent
        if (transform.parent != null)
        {
            
            Debug.Log("Poison affecting the parent: " + transform.parent.name);
            transform.parent.GetComponent<Enemy>().Damage(damage);
            
        }
    }
}