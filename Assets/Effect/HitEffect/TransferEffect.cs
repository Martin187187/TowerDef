using UnityEngine;
public class TransferEffect : HitEffect
{

    public GameObject effectPrefab;
    public override bool Effect(Projectile projectile, Enemy enemy){
        GameObject instantiatedEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        instantiatedEffect.transform.SetParent(enemy.gameObject.transform);

        return false;
    }
}
