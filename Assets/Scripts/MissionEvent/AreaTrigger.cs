using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField]
    private string areaName = "ClearPoint";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MissionEventBus.PublishAreaReached(areaName);
        }
    }
}
