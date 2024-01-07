using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Effector
{
    public int hp = 30;
    public int startHp = 30;

    public void setStartHP()
    {
        startHp = hp;
    }

    
}
