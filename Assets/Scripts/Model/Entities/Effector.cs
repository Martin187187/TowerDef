using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.PlayerLoop;

public abstract class Effector : MonoBehaviour
{

    private float startTime;
    public List<AbstractEffect> initialEffects = new List<AbstractEffect>();
    [HideInInspector] public List<AbstractEffect> effectsRemoval = new List<AbstractEffect>();

    private Transform effectContainer;
    protected EntityManager manager;

    public AbstractEffect GetCompatibleEffect(AbstractEffect effect)
    {
        foreach (var item in GetEffectList())
        {
            if (effect.IsCompatible(item))
            {
                return item;
            }
        }

        return null;
    }
    public void AddEffect(AbstractEffect effect)
    {
        effect.transform.SetParent(effectContainer.transform);
    }

    public void RemoveEffect(AbstractEffect effect)
    {
        effect.transform.SetParent(manager.transform);
    }

    public List<AbstractEffect> GetEffectList()
    {
        var directChildrenEffects = effectContainer.transform
            .Cast<Transform>()
            .SelectMany(child => child.GetComponents<AbstractEffect>())
            .ToList();
        return directChildrenEffects;
    }
    void Awake()
    {
        manager = EntityManager.Instance;
        InitializeEffects();
        startTime = Time.time;
        Init();

    }

    protected virtual void Init()
    {

    }

    public float GetLifetime()
    {
        return Time.time - startTime;
    }
    protected void InitializeEffects()
    {

        GameObject effectsContainer = new GameObject("StaticEffectsContainer");
        effectsContainer.transform.SetParent(transform);
        effectContainer = effectsContainer.transform;
        foreach (AbstractEffect initialEffect in initialEffects)
        {
            AbstractEffect newEffect = Instantiate(initialEffect.gameObject).GetComponent<AbstractEffect>();
            newEffect.transform.SetParent(effectsContainer.transform);

        }
    }

    protected bool PerformOnHit(Effector originEffector, Effector effected, int callingLevel)
    {

        bool stayAlive = false;
        var effects = GetEffectList().OfType<ActionEffect>().ToList();
        effects.Sort();
        Debug.Log("OnHit");
        foreach (var item in effects)
        {
            Debug.Log("sort: " + item.callingLevel);
            if (callingLevel > item.callingLevel)
                continue;
            Debug.Log("sort: " + item.gameObject.name);
            stayAlive |= item.OnHitEffect(originEffector, effected);
        }
        return stayAlive;
    }

    protected void UpdateStaticEffects(Effector effector)
    {

        foreach (var item in GetEffectList().OfType<StaticEffect>())
        {
            item.UpdateEffect(effector);
        }
        foreach (var item in effectsRemoval.OfType<StaticEffect>())
        {
            GetEffectList().Remove(item);
        }
    }
}
