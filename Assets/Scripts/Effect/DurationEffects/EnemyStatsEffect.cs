public abstract class EnemyStatsEffect : AbstractStatsEffect<Enemy>
{
    public enum EnemyStats 
    {
        HEALTH, MOVEMENT_SPEED, DAMAGE
    }

    public EnemyStats modificationStat;
}