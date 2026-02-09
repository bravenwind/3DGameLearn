using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle = 0,
    Patrol = 1,
    Chase = 2,
    Attack = 3,
    Suspicious = 4,
    Dead = 5
}

public class ZombieFSM : MonoBehaviour, IDamageable
{
    [SerializeField]
    [Tooltip("현재 상태")]
    private EnemyState currentState;

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

    [Header("최근 추가된 전투 설정")]
    [SerializeField, Tooltip("공속 (attackRate초에 1번)")] private float attackRate = 1.0f;
    [SerializeField, Tooltip("공격 데미지")] private float attackDamage = 10.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float currentHealth = 0.0f;
    [SerializeField] private float lastAttackTime = 0.0f;
    [SerializeField] private RagdollController ragdollController;

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

        currentHealth = maxHealth;
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead) return;

        switch (currentState)
        {
            case EnemyState.Idle: Update_Idle(); break;
            case EnemyState.Patrol: Update_Patrol(); break;
            case EnemyState.Chase: Update_Chase(); break;
            case EnemyState.Attack: Update_Attack(); break;
            case EnemyState.Suspicious: Update_Suspicious(); break;
        }

        CheckTransitions();
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                idleTimer = 0.0f;
                agent.ResetPath();
                animator.SetBool("IsMoving", false);
                break;

            case EnemyState.Patrol:
                if (wayPoints.Length > 0)
                {
                    agent.SetDestination(wayPoints[currentWaypointIndex].position);
                }
                animator.SetBool("IsMoving", true);
                break;

            case EnemyState.Chase:
                animator.SetBool("IsMoving", true);
                break;

            case EnemyState.Attack:
                // 공격 상태 진입 시 이동 멈춤은 Update_Attack에서 처리
                break;

            case EnemyState.Suspicious:
                suspiciousTimer = 0.0f;
                agent.SetDestination(playerPositionMemory);
                animator.SetBool("IsMoving", true);
                break;

            case EnemyState.Dead:
                Die();
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
        if (wayPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
            ChangeState(EnemyState.Idle);
        }
    }

    void Update_Chase()
    {
        if (agent.enabled && targetPlayer != null)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPlayer.position);
        }
    }

    void Update_Attack()
    {
        if (agent.enabled) agent.isStopped = true;

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

    void CheckTransitions()
    {
        if (targetPlayer == null || currentState == EnemyState.Dead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

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
            if (DetectPlayer_Sight(distanceToPlayer))
            {
                ChangeState(EnemyState.Chase);
            }
            else if (DetectPlayer_Audio(distanceToPlayer))
            {
                ChangeState(EnemyState.Suspicious);
            }
        }
    }

    bool DetectPlayer_Audio(float distance)
    {
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
        if (distance <= viewDistance)
        {
            Vector3 dirToTarget = (targetPlayer.position - eyeTransform.position).normalized;
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

    public void TakeDamage(float damageAmount)
    {
        if (currentState == EnemyState.Dead) return;

        currentHealth -= damageAmount;

        if (currentState != EnemyState.Chase && currentState != EnemyState.Attack)
        {
            ChangeState(EnemyState.Chase);
        }

        if (currentHealth <= 0)
        {
            ChangeState(EnemyState.Dead);
        }
    }

    void Die()
    {
        agent.isStopped = true;
        agent.enabled = false;

        GetComponent<Collider>().enabled = false;

        if (ragdollController != null)
        {
            ragdollController.EnableRagdoll();
        }

        Destroy(gameObject, 5.0f);
    }

    private void OnDrawGizmos()
    {
        if (eyeTransform == null || targetPlayer == null) return;

        Vector3 dirToTarget = (targetPlayer.position - eyeTransform.position).normalized;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(eyeTransform.position, dirToTarget * viewDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}