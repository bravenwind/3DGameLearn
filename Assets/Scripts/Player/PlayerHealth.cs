using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100.0f;

    private float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;

        float healthPercent = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            healthPercent = 0.0f;
        }

        OnHealthChanged?.Invoke(healthPercent);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth < maxHealth) 
        {
            currentHealth = maxHealth;
        }

        float healthPercent = currentHealth / maxHealth;

        OnHealthChanged?.Invoke(healthPercent);
    }

    void Die()
    {
        currentHealth = 0;

        OnDeath?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
