using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public Slider lifeBarSlider;
    private Entity entity;

    void Start()
    {
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
