using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6.0f;

    [SerializeField]
    private float jumpHeight = 2.0f;

    [SerializeField]
    private float gravity = -9.8f;

    [SerializeField]
    private float groundCheckDistance = 0.4f; // 지면에 닿아있는지 체크하기 위한 여유 거리

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float runningMultiplier = 2.0f;

    private Vector3 velocity;
    public bool isGrounded;

    public bool doubleJump;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.Move(move * moveSpeed * runningMultiplier * Time.deltaTime);
        }
        else
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (doubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                doubleJump = false;
            }

            if (isGrounded)
            {
                // 점프 공식 : sqrt(jumpHeight * -2.0f * gravity);
                // 높이만큼 뛰기 위해 필요한 순간 속력을 구하는 물리 공식
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                doubleJump = true;
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
