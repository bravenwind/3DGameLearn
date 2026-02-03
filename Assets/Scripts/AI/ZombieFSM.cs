using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum EnemyState
{
    Idle = 0,
    Patrol = 1,
    Chase = 2,
    Attack = 3,
<<<<<<< Updated upstream
    Suspicious = 4,
    Dead = 5
}

public class ZombieFSM : MonoBehaviour, IDamageable
=======
    Suspicious = 4
}

public class ZombieFSM : MonoBehaviour
>>>>>>> Stashed changes
{
    [SerializeField]
    [Tooltip("현재 상태")]
    private EnemyState currentState;

    //[SerializeField]
    //[Tooltip("감지 거리(이 범위 안에 들어오면 추적 상태로 전이)")]
    //private float detectionRange = 10.0f;

    //[SerializeField]
    //[Tooltip("순찰 반경")]
    //private float patrolRadius = 10.0f;

    [SerializeField]
    private float viewDistance = 15.0f;

    [SerializeField]
    [Tooltip("시야각")]
    private float viewAngle = 60.0f;

    [SerializeField]
    [Tooltip("청각 거리")]
    private float hearingDistance = 5.0f;

    [SerializeField]
    [Tooltip("장애물 레이어")]
    private LayerMask obstacleLayerMask;

    [SerializeField]
    [Tooltip("공격 가능 거리")]
    private float attackRange = 5.0f;

    [SerializeField]
    [Tooltip("웨이포인트 배열")]
    private Transform[] wayPoints;

    [SerializeField]
    [Tooltip("현재 목표 지점 인덱스")]
    private int currentWaypointIndex = 0;

    [SerializeField]
    [Tooltip("플레이어 Transform")]
    private Transform targetPlayer;

    [SerializeField]
    [Tooltip("플레이어 FPSMovement")]
    private FPSMovement playerMovement;

    [SerializeField]
    [Tooltip("적 눈 Transform")]
    private Transform eyeTransform;

    [SerializeField]
    [Tooltip("적 NavMeshAgent")]
    private NavMeshAgent agent;

    [SerializeField]
    [Tooltip("적 Animator")]
    private Animator animator;

<<<<<<< Updated upstream
    [SerializeField, Tooltip("공속 (attackRate초에 1번)")] private float attackRate = 1.0f;
    [SerializeField, Tooltip("공격 데미지")] private float attackDamage = 10.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float currentHealth = 0.0f;
    [SerializeField] private float lastAttackTime = 0.0f;
    [SerializeField] private RagdollController ragdollController;

=======
>>>>>>> Stashed changes
    private float idleTimer = 0.0f;
    private float idleDuration = 2.0f;

    private float suspiciousTimer = 0.0f;
    private float suspiciousDuration = 3.0f;

    private Vector3 playerPositionMemory;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            targetPlayer = go.GetComponent<Transform>();
            playerMovement = go.GetComponent<FPSMovement>();
        }

<<<<<<< Updated upstream
        currentHealth = maxHealth;
=======
>>>>>>> Stashed changes
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                {
                    Update_Idle();
                }
                break;
            case EnemyState.Patrol:
                {
                    Update_Patrol();
                }
                break;
            case EnemyState.Chase:
                {
                    Update_Chase();
                }
                break;
            case EnemyState.Attack:
                {
                    Update_Attack();
                }
                break;
            case EnemyState.Suspicious:
                {
                    Update_Suspicious();
                }
                break;
        }

        CheckTransitions();
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                {
                    idleTimer = 0.0f;
                    agent.ResetPath();
<<<<<<< Updated upstream
=======
                    animator.SetBool("IsAttacking", false);
>>>>>>> Stashed changes
                    animator.SetBool("IsMoving", false);
                }
                break;
            case EnemyState.Patrol:
                {
                    //SetRandomPatrolPoint();
                    if (wayPoints.Length > 0)
                    {
                        agent.SetDestination(wayPoints[currentWaypointIndex].position);
                    }
<<<<<<< Updated upstream
=======
                    animator.SetBool("IsAttacking", false);
>>>>>>> Stashed changes
                    animator.SetBool("IsMoving", true);
                }
                break;
            case EnemyState.Chase:
                {
<<<<<<< Updated upstream
=======
                    animator.SetBool("IsAttacking", false);
>>>>>>> Stashed changes
                    animator.SetBool("IsMoving", true);
                }
                break;
            case EnemyState.Attack:
                {
<<<<<<< Updated upstream

=======
                    animator.SetBool("IsAttacking", true);
>>>>>>> Stashed changes
                }
                break;
            case EnemyState.Suspicious:
                {
                    suspiciousTimer = 0.0f;
                    agent.SetDestination(playerPositionMemory);
<<<<<<< Updated upstream
=======
                    animator.SetBool("IsAttacking", false);
>>>>>>> Stashed changes
                    animator.SetBool("IsMoving", true);
                }
                break;
        }
    }

    void Update_Idle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    void Update_Patrol()
    {
        if (wayPoints.Length == 0)
        {
            return;
        }

        // pathPending : 경로 계산 중인지 여부
        // remainingDistance : 목적지까지 남은 거리
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
            ChangeState(EnemyState.Idle);
        }

    }

    void Update_Chase()
    {
<<<<<<< Updated upstream
        if (agent.enabled == true)
        {
            agent.isStopped = false;
=======
        if (targetPlayer != null)
        {
>>>>>>> Stashed changes
            agent.SetDestination(targetPlayer.position);
        }
    }

    void Update_Attack()
    {
<<<<<<< Updated upstream
        agent.isStopped = true;

        if (targetPlayer != null)
        {
            Vector3 targetPosition = new Vector3(targetPlayer.position.x, transform.position.y, targetPlayer.position.z);

            transform.LookAt(targetPosition);

            if (Time.time >= lastAttackTime + attackRate)
            {
                lastAttackTime = Time.time;

                animator.SetTrigger("Attack");

                IDamageable playerHealth = targetPlayer.GetComponent<IDamageable>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }


=======

    }

>>>>>>> Stashed changes
    void Update_Suspicious()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            animator.SetBool("IsMoving", false);
            suspiciousTimer += Time.deltaTime;
            if (suspiciousTimer >= suspiciousDuration)
            {
                ChangeState(EnemyState.Patrol);
            }
        }
    }

    /// <summary>
    /// 랜덤한 순찰 지점 찾는 함수
    /// </summary>
    void SetRandomPatrolPoint()
    {
        // 랜덤 방향 계산
        //Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        //randomDirection += transform.position;

        //NavMeshHit hit;

        //// 생성한 랜덤 좌표가 NavMesh 위의 유효한 좌표인지 확인
        //if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        //{
        //    agent.SetDestination(hit.position);
        //}
    }

    /// <summary>
    /// 조건 체크로 상태 전이
    /// </summary>
    void CheckTransitions()
    {
        if (targetPlayer == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

<<<<<<< Updated upstream
        if (currentState == EnemyState.Chase)
        {
            if (distanceToPlayer <= attackRange)
            {
                ChangeState(EnemyState.Attack);
            }
            else if (distanceToPlayer > viewDistance)
            {
                ChangeState(EnemyState.Patrol);
            }
        }
        else if (currentState == EnemyState.Attack)
        {
            if (distanceToPlayer > attackRange)
            {
                ChangeState(EnemyState.Chase);
            }
        }
        else
        {
            if (DetectPlayer_Sight(distanceToPlayer) || DetectPlayer_Audio(distanceToPlayer))
            {
                ChangeState(EnemyState.Chase);
=======
        if (distanceToPlayer <= attackRange) 
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        if (currentState == EnemyState.Attack)
        {
            ChangeState(EnemyState.Chase);
            return; // 여기서 바로 리턴하거나 아래 로직을 태울 수 있음
        }


        //if (distanceToPlayer <= detectionRange && currentState != EnemyState.Chase) 
        //{
        //    ChangeState(EnemyState.Chase);
        //}
        //else if (distanceToPlayer > detectionRange && currentState == EnemyState.Chase)
        //{
        //    ChangeState(EnemyState.Patrol);
        //}

        if (currentState != EnemyState.Chase)
        {
            if (DetectPlayer_Sight(distanceToPlayer))
            {
                ChangeState(EnemyState.Chase);
            }

            if (DetectPlayer_Audio(distanceToPlayer))
            {
                ChangeState(EnemyState.Suspicious);
            }
        }
        else
        {
            if (distanceToPlayer > viewDistance)
            {
                ChangeState(EnemyState.Patrol);
>>>>>>> Stashed changes
            }
        }
    }

    /// <summary>
    /// 시각 및 청각 감지 여부를 판단
    /// </summary>
    bool DetectPlayer_Audio(float distance)
    {
        // 청각 감지 (거리 + 플레이어 이동 여부)
        // 등 뒤에 있어도 가깝고, 플레이어가 움직이면 감지
        if (distance <= hearingDistance)
        {
            if (playerMovement != null && playerMovement.IsMoving())
            {
                playerPositionMemory = playerMovement.transform.position;
                return true;
            }
        }

        return false;
    }

    bool DetectPlayer_Sight(float distance) 
    {
        // 시각 감지 (거리 + 시야각 + 장애물)
        if (distance <= viewDistance) 
        {
            Vector3 dirToTarget = (targetPlayer.position - eyeTransform.position).normalized;

            // 자신의 정면과 타겟 방향 사이의 각도
            float angle = Vector3.Angle(eyeTransform.forward, dirToTarget);

            if (angle <= viewAngle * 0.5f)
            {
                if (!Physics.Raycast(eyeTransform.position, dirToTarget, viewDistance, obstacleLayerMask))
                {
                    return true;
                }
            }
        }

        return false;  
    }

<<<<<<< Updated upstream
    public void TakeDamage(float damageAmount)
    {
        if (currentState == EnemyState.Dead)
        {
            return;
        }

        currentHealth -= damageAmount;

        if (currentState != EnemyState.Chase && currentState != EnemyState.Attack)
        {
            ChangeState(EnemyState.Chase);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ChangeState(EnemyState.Dead);

        agent.isStopped = true;
        agent.enabled = false;

        GetComponent<Collider>().enabled = false;

        if (ragdollController != null)
        {
            ragdollController.EnableRagdoll();
        }

        Destroy(gameObject, 5.0f);
    }

=======
>>>>>>> Stashed changes
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 dirToTarget = (targetPlayer.position - eyeTransform.position).normalized;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(eyeTransform.position, dirToTarget * viewDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
