public abstract class TurrestStatsEffect : AbstractStatsEffect<Turret>
{
    public enum TurretStats 
    {
        ATTACK_DAMGE, ATTACK_INTERVAL, ROTATION_SPEED, RANGE
    }

    public TurretStats modificationStat;

    public override bool IsCompatible(AbstractEffect obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        TurrestStatsEffect other = (TurrestStatsEffect)obj;

        return modificationStat == other.modificationStat;
    }
}