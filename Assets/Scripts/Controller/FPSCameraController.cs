using UnityEngine;
using UnityEngine.UI;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 200.0f;

    [SerializeField]
    private Transform playerBody;

    public Toggle mouseYReverseToggle;
    public Slider rotLimitSlider;

    public bool mouseYReversed;
    private float xRotation = 0.0f;

    public float rotLimitation = 90.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (mouseYReversed)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        xRotation = Mathf.Clamp(xRotation, -rotLimitation, rotLimitation);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void OnToggleClicked()
    {
        mouseYReversed = mouseYReverseToggle.isOn;
    }

    public void OnSliderValueChanged()
    {
        rotLimitation = rotLimitSlider.value;
    }
}
