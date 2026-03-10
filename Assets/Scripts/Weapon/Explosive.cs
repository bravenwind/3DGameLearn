using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField]
    protected float explosionRadius = 5.0f;

    [SerializeField]
    protected float explosionForce = 700.0f;

    [SerializeField]
    protected float maxDamage = 100.0f;

    [SerializeField]
    protected float minDamage = 20.0f;

    [SerializeField]
    protected GameObject explosionEffect;

    [SerializeField]
    protected LayerMask damageLayer;

    [SerializeField]
    protected LayerMask explosionLayer;

    [SerializeField]
    protected bool hasExploded = false;

    protected void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Collider[] cols_Damage = Physics.OverlapSphere(transform.position, explosionRadius, damageLayer);

        // 1차 루프: 데미지부터 전부 먼저 줍니다.
        // 이 과정에서 죽을 좀비들은 Die()가 호출되어 랙돌(isKinematic = false)이 켜집니다.
        for (int i = 0; i < cols_Damage.Length; i++)
        {
            IDamageable damageable = cols_Damage[i].GetComponentInParent<IDamageable>(); // 랙돌 부위를 맞아도 부모의 데미지 인터페이스를 찾도록 변경
            if (damageable != null)
            {
                float dist = Vector3.Distance(transform.position, cols_Damage[i].ClosestPoint(transform.position));
                float finalDamage = Mathf.Lerp(maxDamage, minDamage, dist / explosionRadius);
                damageable.TakeDamage(finalDamage);
            }
        }

        Collider[] cols_Explosion = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayer);

        // 2차 루프: 폭발력을 가합니다.
        // 이제 랙돌이 켜진 부위들은 isKinematic이 false이므로 정상적으로 날아갑니다.
        for (int i = 0; i < cols_Explosion.Length; i++)
        {
            Rigidbody rb = cols_Explosion[i].GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic) // Kinematic이 풀린(랙돌화된) 부위나 일반 물리 객체만 날림
            {
                Debug.Log(rb.gameObject.name);
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    protected void SpawnEffect()
    {
        if (explosionEffect != null)
        {
            GameObject go = Instantiate(explosionEffect, transform.position, transform.rotation);
            if (go != null)
            {
                Destroy(go, 3.0f);
            }
        }
    }
}
