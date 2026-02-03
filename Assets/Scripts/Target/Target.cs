using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float health = 50.0f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(transform.name + " 남은 체력: "+ health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
