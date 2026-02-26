using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void Enter(ZombieFSM zombie)
    {
        if (zombie.WayPoints.Length > 0)
        {
            zombie.Agent.SetDestination(zombie.WayPoints[zombie.CurrentWaypointIndex].position);
        }
        zombie.Animator.SetBool("IsMoving", true);
    }

    public override void Update(ZombieFSM zombie)
    {
        // 1. 상태 전환 조건 검사
        if (zombie.TargetPlayer != null)
        {
            float distance = Vector3.Distance(zombie.transform.position, zombie.TargetPlayer.position);
            if (zombie.DetectPlayer_Sight(distance)) { zombie.ChangeState(zombie.StateChase); return; }
            if (zombie.DetectPlayer_Audio(distance)) { zombie.ChangeState(zombie.StateSuspicious); return; }
        }

        // 2. 순찰 행동
        if (zombie.WayPoints.Length == 0) return;

        if (!zombie.Agent.pathPending && zombie.Agent.remainingDistance < 0.5f)
        {
            zombie.CurrentWaypointIndex = (zombie.CurrentWaypointIndex + 1) % zombie.WayPoints.Length;
            zombie.ChangeState(zombie.StateIdle);
        }
    }

    public override void Exit(ZombieFSM zombie) { }
}