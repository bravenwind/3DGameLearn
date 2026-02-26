using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void Enter(ZombieFSM zombie)
    {
        zombie.Animator.SetBool("IsMoving", true);
    }

    public override void Update(ZombieFSM zombie)
    {
        if (zombie.TargetPlayer == null) return;

        float distance = Vector3.Distance(zombie.transform.position, zombie.TargetPlayer.position);

        // 1. 상태 전환 조건 검사
        if (distance <= zombie.AttackRange)
        {
            zombie.ChangeState(zombie.StateAttack);
            return;
        }
        else if (distance > zombie.ViewDistance)
        {
            zombie.ChangeState(zombie.StatePatrol);
            return;
        }

        // 2. 추적 행동
        if (zombie.Agent.enabled)
        {
            zombie.Agent.isStopped = false;
            zombie.Agent.SetDestination(zombie.TargetPlayer.position);
        }
    }

    public override void Exit(ZombieFSM zombie) { }
}