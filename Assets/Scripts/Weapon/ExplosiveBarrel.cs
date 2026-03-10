using UnityEngine;

public class ExplosiveBarrel : Explosive, IDamageable
{
    [SerializeField]
    private float health = 50.0f;

    private bool isExploded = false;

    public void TakeDamage(float damageAmount)
    {
        if (isExploded) 
        {
            return;
        }

        health -= damageAmount;
        if (health < 0)
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
