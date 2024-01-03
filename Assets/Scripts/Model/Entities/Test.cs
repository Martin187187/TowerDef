using System.Collections.Generic;

public class Test : Effector
{

    protected List<ActionEffect<Test>> actionEffects = new List<ActionEffect<Test>>();
    private List<AbstractStatsEffect<Test>> statsEffectsValue = new List<AbstractStatsEffect<Test>>();
    public float value;
    
    public float CalculateValue()
    {
        float sum = value;
        foreach (var item in statsEffectsValue)
        {
            sum+=item.getAdditionValue(this);
        }

        return sum;
    }


    protected bool OnAction()
    {
        bool stayAlive = false;
        foreach (var item in actionEffects)
        {
            stayAlive |= item.OnHitEffect(this);
        }
        return stayAlive;
    }

    protected void performUpdate()
    {
        foreach (var item in actionEffects)
        {
            item.UpdateEffect();
        }
    }

}