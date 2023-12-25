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
            Enemy enemy = transform.parent.GetComponent<Enemy>();
            enemy.Damage((int)(enemy.hp*0.05f));
            
        }
    }
}