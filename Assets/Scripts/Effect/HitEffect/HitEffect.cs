using UnityEngine;
public abstract class HitEffect : MonoBehaviour
{

    public abstract bool Effect(Projectile projectile, Enemy enemy);
}
