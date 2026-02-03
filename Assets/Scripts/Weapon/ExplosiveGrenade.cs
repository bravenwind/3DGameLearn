using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius = 5.0f;

    [SerializeField]
    private float explosionForce = 700.0f;

    [SerializeField]
    private float explodeTime = 1.0f;

    [SerializeField]
    private float explodeTimer = 0.0f;

    [SerializeField]
    private bool triggered = false;

    [SerializeField]
    private float maxDamage = 50.0f;

    [SerializeField]
    private float minDamage = 10.0f;

    [SerializeField]
    private GameObject explosionEffect;


    private void OnCollisionEnter(Collision collision)
    {
        triggered = true;
    }

    private void Update()
    {
        if (!triggered) 
        {
            return;
        }

        explodeTimer += Time.deltaTime;

        if (explodeTimer >= explodeTime)
        {
            Explode();
            SpawnEffect();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Æø¹ß Ã³¸®
    /// </summary>
    void Explode()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < cols.Length; i++)
        {
            Target target = cols[i].GetComponent<Target>();
            if (target != null)
            {
                float dist = Vector3.Distance(transform.position, target.gameObject.transform.position);
                float finalDamage = Mathf.Lerp(maxDamage, minDamage, dist / explosionRadius);
                target.TakeDamage(finalDamage);
            }

            //IDamageable damageable = cols[i].GetComponent<IDamageable>();
            //if (damageable != null)
            //{
            //    damageable.TakeDamage(damage);
            //}

            Rigidbody rb = cols[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    //IEnumerator SpawnEffectAndDestroy()
    //{
    //    explodeTimer += Time.deltaTime;

    //    while (explodeTimer < explodeTime)
    //    {
    //        yield return null;
    //    }



    //    Destroy(gameObject);
    //    explodeTimer = 0.0f;
    //    yield break;
    //}

    void SpawnEffect()
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
