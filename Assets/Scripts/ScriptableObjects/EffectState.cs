using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectState", menuName = "ScriptableObjects/EffectState")]
public class EffectState : ScriptableObject
{
    [SerializeField] private List<GameObject> effectList = new List<GameObject>();
    public System.Action OnListChanged;

    public List<AbstractEffect> GetEffects()
    {
        List<AbstractEffect> abstractEffects = new List<AbstractEffect>();
        foreach (GameObject obj in effectList)
        {
            AbstractEffect effect = obj.GetComponent<AbstractEffect>();
            if (effect != null)
            {
                abstractEffects.Add(effect);
            }
        }
        return abstractEffects;
    }

    public void AddEffect(AbstractEffect effect)
    {
        effectList.Add(effect.gameObject);
        OnListChanged?.Invoke();
    }
    public void RemoveEffect(AbstractEffect effect)
    {
        effectList.Remove(effect.gameObject);
        OnListChanged?.Invoke();
    }
}
