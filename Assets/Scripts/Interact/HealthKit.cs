using UnityEngine;

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float healAmount = 50.0f;

    public string GetInteractText()
    {
        return "[F] 구급상자 사용 " + healAmount.ToString() + " HP";
    }

    public void Interact(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null) 
        {
            health.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
