using UnityEngine;

public class TransferActionTurretEffect : ActionEffect
{
    public AbstractEffect effectToInject;

    void Awake()
    {
        effectToInject = Instantiate(effectToInject.gameObject).GetComponent<AbstractEffect>();
        effectToInject.transform.parent = transform;
    }

    public override void ConsumeOtherObject(AbstractEffect otherObject)
    {
        if (!IsCompatible(otherObject))
        {
            Debug.LogWarning("You tried to combine non compatible Effects.");
            return;
        }
        TransferActionTurretEffect otherEffect = (TransferActionTurretEffect)otherObject;
        effectToInject.ConsumeOtherObject(otherEffect.effectToInject);
        Destroy(otherEffect.gameObject);
    }
    public override bool IsCompatible(AbstractEffect obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        TransferActionTurretEffect other = (TransferActionTurretEffect)obj;

        return effectToInject.IsCompatible(other.effectToInject);
    }

    public override bool OnHitEffect(Effector originEffector, Effector effected)
    {
        if (effected)
        {
            AbstractEffect compatibleEffect = effected.GetCompatibleEffect(effectToInject);
            if(compatibleEffect){
                effected.RemoveEffect(compatibleEffect);
                Destroy(compatibleEffect.gameObject);
            }
            effected.AddEffect(Instantiate(effectToInject.gameObject).GetComponent<AbstractEffect>());
        }
        return false;
    }

}