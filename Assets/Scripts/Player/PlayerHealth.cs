using UnityEngine;
using Unity.Cinemachine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100.0f;

    [SerializeField]
    private CinemachineCamera deathCam;

    [SerializeField]
    private LayerMask deathCamLayerMask;

    [SerializeField]
    private FPSCameraController controller;

    [SerializeField]
    private FPSMovement movement;

    [SerializeField]
    private HitScanWeapon weapon;

    [SerializeField]
    private GameObject uiPanel;

    private float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = GameManager.Instance.currentDifficultyData.playerHP;

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

        controller.enabled = false;
        movement.enabled = false;
        weapon.enabled = false;
        uiPanel.SetActive(false);

        Camera.main.cullingMask = deathCamLayerMask;

        deathCam.Priority = 10;

    }
}
