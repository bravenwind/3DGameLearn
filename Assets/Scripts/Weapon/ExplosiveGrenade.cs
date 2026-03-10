using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class ExplosiveGrenade : Explosive
{
    [SerializeField]
    private float explodeTime = 1.0f;

    [SerializeField]
    private float explodeTimer = 0.0f;

    [SerializeField]
    private bool triggered = false;

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
