using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField]
    private float swayMultiplier = 0.02f; // 흔들림 강도

    [SerializeField]
    private float maxAmount = 0.06f;      // 최대 이동 제한

    [SerializeField]
    private float smooth = 6.0f;          // 부드러운 정도

    private Vector3 initialPosition;      // 초기 위치 저장용 변수

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        // 이동 범위 제한
        mouseX = Mathf.Clamp(mouseX, -maxAmount, maxAmount);
        mouseY = Mathf.Clamp(mouseY, -maxAmount, maxAmount);

        // 목표 위치 계산
        Vector3 finalPosition = new Vector3(mouseX, mouseY, 0) + initialPosition;

        // 목표 회전값 계산 (입력의 반대 방향 + 위아래 / 좌우 고려)
        // 마우스를 오른쪽으로 움직이면 무기는 왼쪽으로 회전 (Y축 회전) 해야 자연스럽다.

        //Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        //Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        //Quaternion targetRotation = rotationX * rotationY;

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, smooth * Time.deltaTime);
        
        // 현재 회전값에서 목표 회전값으로 부드럽게 회전
        // 회전이기 때문에 Lerp 함수 사용 X, Slerp 함수 사용
    }
}
