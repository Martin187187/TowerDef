using UnityEngine;
public class TransferEffect : HitEffect
{

    public GameObject effectPrefab;
    public override bool Effect(Projectile projectile, Enemy enemy)
    {
        if(enemy == null)
            return false;

        foreach (Transform child in enemy.transform)
        {
            if (effectPrefab.GetComponent<PoisonEffect>() != null && child.gameObject.GetComponent<PoisonEffect>() != null)
            {
                return false;
            }
        }
        GameObject instantiatedEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        instantiatedEffect.transform.SetParent(enemy.gameObject.transform);

        return false;
    }
}
