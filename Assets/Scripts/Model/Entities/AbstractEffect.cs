using UnityEngine;
public abstract class AbstractEffect : MonoBehaviour
{
    public bool isUserInteractible = true;
    public string effectName = "";
    public string description = "";
    public abstract void ConsumeOtherObject(AbstractEffect otherObject);
    public Sprite sprite;

    public virtual bool IsCompatible(AbstractEffect obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }


        return true;
    }
}