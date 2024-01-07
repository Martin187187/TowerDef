using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Transform dropInto;

    public bool isBag;
    private EntityManager manager;
    public TurretViewUI ui;

    void Start()
    {
        manager = EntityManager.Instance;

    }
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggableItem != null)
        {
            if (isBag)
            {
                manager.AddEffect(draggableItem.content);
                ui.targetTurret.RemoveEffect(draggableItem.content);
            }
            else
            {
                manager.RemoveEffect(draggableItem.content);
                ui.targetTurret.AddEffect(draggableItem.content);
            }
            ui.Recalculate();

        }
    }

    public void ClearChilds(List<AbstractEffect> effects)
    {
        List<AbstractEffect> remain = new List<AbstractEffect>();
        for (int i = 0; i < dropInto.childCount; i++)
        {

            var a = dropInto.GetChild(i).gameObject;
            DraggableItem d = a.GetComponent<DraggableItem>();
            if (!effects.Contains(d.content))
            {
                d.EndDrag();
                Destroy(a);
            } 
            else
            {
                remain.Add(d.content);
            }
        }
        foreach (var item in effects)
        {
            if(!remain.Contains(item))
            manager.createItem(item, dropInto);
        }
    }
}
