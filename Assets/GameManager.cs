using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DifficultyDataSO easyData;
    public DifficultyDataSO hardData;
    public DifficultyDataSO currentDifficultyData;

    [Header("Enemy Body Partial Damage Multiplier")]
    public float headDamageMultiplier = 3.0f;
    public float bodyDamageMultiplier = 1.0f;
    public float legDamageMultiplier = 0.7f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}