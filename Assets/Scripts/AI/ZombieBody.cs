using UnityEngine;

public enum BodyType
{
    Head,
    Body,
    Leg
}

public class ZombieBody : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ZombieFSM fsm;

    [SerializeField]
    private BodyType type;

    [SerializeField]
    private float damageMultiplier = 1.0f;

    private void Start()
    {
        switch (type)
        {
            case BodyType.Head:
                damageMultiplier = GameManager.Instance.headDamageMultiplier;
                break;
            case BodyType.Body:
                damageMultiplier = GameManager.Instance.bodyDamageMultiplier;
                break;
            case BodyType.Leg:
                damageMultiplier = GameManager.Instance.legDamageMultiplier;
                break;
            default:
                damageMultiplier = GameManager.Instance.bodyDamageMultiplier;
                break;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        float finalDamage = damageAmount * damageMultiplier;

        bool isLegHit = type == BodyType.Leg;

        fsm.TakePartialDamage(finalDamage, isLegHit);
    }
}
