using UnityEngine;

public class EnemySuspiciousState : EnemyBaseState
{
    private float suspiciousTimer;

    public override void Enter(ZombieFSM zombie)
    {
        suspiciousTimer = 0.0f;
        zombie.Agent.SetDestination(zombie.PlayerPositionMemory);
        zombie.Animator.SetBool("IsMoving", true);
    }

    public override void Update(ZombieFSM zombie)
    {
        // 1. 시야에 플레이어가 들어오면 즉시 추적
        if (zombie.TargetPlayer != null)
        {
            float distance = Vector3.Distance(zombie.transform.position, zombie.TargetPlayer.position);
            if (zombie.DetectPlayer_Sight(distance)) { zombie.ChangeState(zombie.StateChase); return; }
        }

        // 2. 기억한 위치로 이동 후 두리번거리기
        if (!zombie.Agent.pathPending && zombie.Agent.remainingDistance < 0.5f)
        {
            zombie.Animator.SetBool("IsMoving", false);
            suspiciousTimer += Time.deltaTime;
            if (suspiciousTimer >= zombie.SuspiciousDuration)
            {
                zombie.ChangeState(zombie.StatePatrol);
            }
        }
    }

    public override void Exit(ZombieFSM zombie) { }
}
