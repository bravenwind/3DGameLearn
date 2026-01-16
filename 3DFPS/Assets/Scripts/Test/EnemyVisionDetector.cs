using UnityEngine;

public class EnemyVisionDetector : MonoBehaviour
{
    public Transform target;

    public float detectionRadius = 10.0f; // 감지 거리
    
    public float viewAngle = 90.0f;       // 시야각 (전방 기준 좌우 벌어진 각도)

    private void Start()
    {
        
    }

    private void Update()
    {
        DetectTarget();
    }

    /// <summary>
    /// 플레이어를 감지하는 함수
    /// </summary>
    void DetectTarget()
    {
        // 적 -> 플레이어로 향하는 벡터 계산
        // 목적지 - 출발지
        // 벡터의 뺄셈을 이용해서 적에게서 플레이어로 가는 벡터를 만듬. 벡터는 방향과 거리 정보 모두 담고 있음.
        Vector3 directionToTarget = target.position - transform.position;

        // 거리 계산 (벡터 길이)
        // magnitude는 벡터의 길이를 반환한다 - 피타고라스 정리
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > detectionRadius) 
        {
            return;
        }

        // 방향 벡터 정규화
        // 내적을 정확히 계산하기 위해 길이를 1로 만듬
        Vector3 normalizeDirection = directionToTarget.normalized;

        // 내적 계산
        // 적의 정면과 타겟 방향 간의 관계 계산.
        float dotResult = Vector3.Dot(transform.forward, normalizeDirection);
    
        // 내적 결과를 각도로 변환
        float angleToTarget = Mathf.Acos(dotResult) * Mathf.Rad2Deg;

        // 시야각 판정
        // 전체 시야각의 절반보다 작은 각도에 있어야 시야 내에 있는 것
        if (angleToTarget <= viewAngle * 0.5f)
        {
            Debug.Log("플레이어 발견!");
            Debug.DrawLine(transform.position, target.position, Color.red);
            transform.LookAt(target);
        }
        else
        {
            Debug.Log("플레이어 발견 못함");
            Debug.DrawLine(transform.position, target.position, Color.yellow);
        }
    }
}
