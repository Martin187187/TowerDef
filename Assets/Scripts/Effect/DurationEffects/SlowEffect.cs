using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : Effect
{
    public float slowRatio = 0.5f;
    protected override void EffectParent()
    {
        // Check if the GameObject has a parent
        if (transform.parent != null)
        {
            
        }
    }
}