public abstract class ProjectileStatsEffect : AbstractStatsEffect<Projectile>
{
    public enum ProjectileStats 
    {
        SPEED, DAMAGE, LIFETIME
    }

    public ProjectileStats modificationStat;
}