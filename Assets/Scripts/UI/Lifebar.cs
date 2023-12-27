using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public Slider lifeBarSlider;
    public Entity entity;

    void Start()
    {
        if(entity==null)
            entity = transform.parent.GetComponent<Entity>();
    }

    void Update()
    {
        lifeBarSlider.value = (float)entity.hp / entity.startHp;
    }

    void LateUpdate()
    {
        // Keep the child's rotation constant
        entity.transform.rotation = Quaternion.identity;
    }
}
