using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private float interactRange = 3.0f;  // 손이 닿는 거리

    [SerializeField]
    private LayerMask interactableMask;  // 아이템 레이어만 검사

    [SerializeField]
    private TMP_Text interactPromptText; // 화면 중앙 안내 텍스트

    private void Start()
    {
        interactPromptText.text = string.Empty;
    }

    private void Update()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        bool doHit = Physics.Raycast(ray, out hit, interactRange, interactableMask);

        if (doHit)
        {
            IInteractable interactObj = hit.transform.GetComponent<IInteractable>();
            if (interactObj != null)
            {
                if (interactPromptText != null)
                {
                    interactPromptText.text = interactObj.GetInteractText();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactObj.Interact(gameObject);
                }
                return;
            }
        }

        if (interactPromptText != null)
        {
            interactPromptText.text = string.Empty;
        }
    }
}
