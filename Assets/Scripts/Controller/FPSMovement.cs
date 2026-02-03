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

    private Vector3 verticalVelocity;
    public bool isGrounded;

    public bool doubleJump;
    public bool isMoving;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        // [수정 1] 움직임 체크 위치 변경 (이동 로직 후, 혹은 Move 합친 후 확인)
        // 하지만 CharacterController.velocity는 물리 연산 후 갱신되므로 
        // 여기서 체크하려면 입력값을 기반으로 하거나, 이전 프레임의 속도를 쓰게 됩니다.
        // 가장 확실한 건 입력값 확인입니다.

        if (isGrounded && verticalVelocity.y <= 0)
        {
            verticalVelocity.y = -2.0f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 1. 수평 이동 벡터 계산
        Vector3 moveDir = transform.right * x + transform.forward * z;
        Vector3 finalMove = moveDir.normalized * moveSpeed; // 정규화하여 대각선 이동 속도 일정하게

        if (Input.GetKey(KeyCode.LeftControl))
        {
            finalMove *= runningMultiplier;
        }

        // 2. 점프 및 중력 (수직 이동) 계산
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (doubleJump)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                doubleJump = false;
            }
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                doubleJump = true;
            }
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        // 3. [핵심 수정] 수평 + 수직 벡터를 합침
        // finalMove(수평 속도) + verticalVelocity(수직 속도)
        // Time.deltaTime은 Move 호출 시 한 번에 적용하는 것이 깔끔함
        Vector3 finalVelocity = finalMove + verticalVelocity;

        // 4. Move는 한 번만 호출
        controller.Move(finalVelocity * Time.deltaTime);

        // 5. 이동 여부 판별
        // CharacterController의 velocity 대신 입력값이나 수평 속도 벡터로 판단하는 것이 더 반응성이 좋습니다.
        // 단순히 "키보드를 눌렀는가?"로 판단하는 것이 애니메이션/소리 처리에 더 직관적입니다.
        // isMoving = moveDir.magnitude > 0; 

        // 만약 물리적인 이동을 꼭 체크해야 한다면 (벽에 막힘 등 포함):
        // 수직 속도(중력)는 제외하고 수평 속도만으로 판단해야 정확합니다.
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        isMoving = horizontalVelocity.sqrMagnitude > 0.1f;
    }

    /// <summary>
    /// 이동 중인지 여부를 반환
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return isMoving;
    }
}
