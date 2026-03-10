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
    [Tooltip("현재 상태 (인스펙터 확인용 Enum)")]
    private EnemyState currentStateType;

    [SerializeField] private float viewDistance = 15.0f;
    [SerializeField] private float viewAngle = 60.0f;
    [SerializeField] private float hearingDistance = 5.0f;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float attackRange = 5.0f;

    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private int currentWaypointIndex = 0;

    [SerializeField] private Transform targetPlayer;
    [SerializeField] private FPSMovement playerMovement;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [Header("최근 추가된 전투 설정")]
    [SerializeField] private float attackRate = 1.0f;
    [SerializeField] private float attackDamage = 10.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float currentHealth = 0.0f;
    [SerializeField] private float lastAttackTime = 0.0f;
    [SerializeField] private RagdollController ragdollController;

    [Header("타이머 설정 (인스펙터 조절용)")]
    [SerializeField] private float idleDuration = 2.0f;
    [SerializeField] private float suspiciousDuration = 3.0f;

    private Vector3 playerPositionMemory;

    // --- 상태 패턴용 인스턴스 및 프로퍼티 ---
    public EnemyBaseState CurrentState { get; private set; }

    public EnemyIdleState StateIdle { get; private set; }
    public EnemyPatrolState StatePatrol { get; private set; }
    public EnemyChaseState StateChase { get; private set; }
    public EnemyAttackState StateAttack { get; private set; }
    public EnemySuspiciousState StateSuspicious { get; private set; }
    public EnemyDeadState StateDead { get; private set; }

    // 외부 상태 클래스들이 본체의 데이터에 접근할 수 있도록 열어둔 프로퍼티(Property)
    public NavMeshAgent Agent => agent;
    public Animator Animator => animator;
    public Transform TargetPlayer => targetPlayer;
    public Transform[] WayPoints => wayPoints;
    public float ViewDistance => viewDistance;
    public float AttackRange => attackRange;
    public float AttackRate => attackRate;
    public float IdleDuration => idleDuration;
    public float SuspiciousDuration => suspiciousDuration;

    public int CurrentWaypointIndex { get => currentWaypointIndex; set => currentWaypointIndex = value; }
    public Vector3 PlayerPositionMemory { get => playerPositionMemory; set => playerPositionMemory = value; }
    public float LastAttackTime { get => lastAttackTime; set => lastAttackTime = value; }

    private void Start()
    {
        maxHealth = GameManager.Instance.currentDifficultyData.enemyHP;
        attackDamage = GameManager.Instance.currentDifficultyData.enemyAttack;
        ragdollController = GetComponent<RagdollController>();

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            targetPlayer = go.GetComponent<Transform>();
            playerMovement = go.GetComponent<FPSMovement>();
        }

        currentHealth = maxHealth;

        // 상태 인스턴스 초기화
        StateIdle = new EnemyIdleState();
        StatePatrol = new EnemyPatrolState();
        StateChase = new EnemyChaseState();
        StateAttack = new EnemyAttackState();
        StateSuspicious = new EnemySuspiciousState();
        StateDead = new EnemyDeadState();

        // 초기 상태 진입
        ChangeState(StateIdle);
    }

    private void Update()
    {
        CurrentState?.Update(this);
    }

    public void ChangeState(EnemyBaseState newState)
    {
        if (CurrentState == newState) return;

        // 기존 상태의 Exit 로직 실행
        CurrentState?.Exit(this);

        // 상태 교체
        CurrentState = newState;

        // 인스펙터 디버깅용 Enum 동기화
        if (newState == StateIdle) currentStateType = EnemyState.Idle;
        else if (newState == StatePatrol) currentStateType = EnemyState.Patrol;
        else if (newState == StateChase) currentStateType = EnemyState.Chase;
        else if (newState == StateAttack) currentStateType = EnemyState.Attack;
        else if (newState == StateSuspicious) currentStateType = EnemyState.Suspicious;
        else if (newState == StateDead) currentStateType = EnemyState.Dead;

        // 새로운 상태의 Enter 로직 실행
        CurrentState.Enter(this);
    }

    public bool DetectPlayer_Audio(float distance)
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

    public bool DetectPlayer_Sight(float distance)
    {
        if (distance <= viewDistance)
        {
            Vector3 dirToTarget = (targetPlayer.position - eyeTransform.position).normalized;
            float angle = Vector3.Angle(eyeTransform.forward, dirToTarget);

            if (angle <= viewAngle * 0.5f)
            {
                if (!Physics.Raycast(eyeTransform.position, dirToTarget, distance, obstacleLayerMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void TakeDamage(float damageAmount)
    {
        if (CurrentState == StateDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            ChangeState(StateDead);
        }
        else if (CurrentState != StateChase && CurrentState != StateAttack)
        {
            ChangeState(StateChase);
        }
    }

    public void Die()
    {
        agent.isStopped = true;
        agent.enabled = false;

        GetComponent<Collider>().enabled = false;

        if (ragdollController != null)
        {
            Debug.Log("랙돌 활성화");
            ragdollController.EnableRagdoll();
        }

        MissionEventBus.PublishEnemyKilled();

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

    public void OnAnimationHit()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        // [수정됨] 실제 공격이 적중하는 범위는 상태 진입 범위(attackRange)보다 약간(20%) 넉넉하게 주어 회피 판정을 부드럽게 만듭니다.
        if (distanceToPlayer <= attackRange * 1.2f)
        {
            IDamageable playerHealth = targetPlayer.GetComponent<IDamageable>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    public void DetectStoneAudio(Vector3 pos)
    {
        playerPositionMemory = pos;
        ChangeState(StateSuspicious);
    }

    public void TakePartialDamage(float damageAmount, bool isLegHit)
    {
        if (CurrentState == StateDead)
        {
            return;
        }

        TakeDamage(damageAmount);

        if (currentHealth > 0 && isLegHit && agent.enabled)
        {
            agent.speed *= 0.5f;
            animator.speed *= 0.5f;
        }
    }
}