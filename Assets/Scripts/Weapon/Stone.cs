using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private bool isTouched = false;

    [SerializeField]
    private float detectRange = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (isTouched)
        {
            return;
        }

        isTouched = true;
        Debug.Log("∂•ø° ¥Í¿Ω");
        Collider[] cols = Physics.OverlapSphere(transform.position, detectRange, enemyLayer);

        foreach (Collider col in cols) 
        {
            ZombieFSM zombie = col.gameObject.GetComponent<ZombieFSM>();
            zombie.DetectStoneAudio(transform.position);
        }

        Destroy(gameObject, 1.5f);
    }
}
