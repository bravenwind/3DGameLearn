using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DifficultyDataSO easyData;
    public DifficultyDataSO hardData;
    public DifficultyDataSO currentDifficultyData;

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