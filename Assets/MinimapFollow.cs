using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField]
    private Transform playerTarget;

    [SerializeField]
    private float cameraHeight = 30.0f;

    [SerializeField]
    private bool rotateWithPlayer = false;

    private void LateUpdate()
    {
        if (playerTarget == null) return;

        Vector3 newPosition = playerTarget.position;
        newPosition.y = cameraHeight;
        transform.position = newPosition;

        if (rotateWithPlayer )
        {
            transform.rotation = Quaternion.Euler(90.0f, playerTarget.eulerAngles.y, 0.0f);
        }
    }
}
