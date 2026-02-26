using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/DifficultyData")]
public class DifficultyDataSO : ScriptableObject
{
    public float playerHP;
    public float playerAttack;
    public float enemyHP;
    public float enemyAttack;
}
