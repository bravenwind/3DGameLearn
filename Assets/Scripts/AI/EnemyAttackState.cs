using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void Enter(ZombieFSM zombie)
    {
        // 공격 상태 진입 시 이동 멈춤
        if (zombie.Agent.enabled) zombie.Agent.isStopped = true;
    }

    public override void Update(ZombieFSM zombie)
    {
        if (zombie.TargetPlayer == null) return;

        float distance = Vector3.Distance(zombie.transform.position, zombie.TargetPlayer.position);

        // 1. 상태 전환 조건 검사
        if (distance > zombie.AttackRange)
        {
            zombie.ChangeState(zombie.StateChase);
            return;
        }

        // 2. 공격 행동
        Vector3 targetPosition = new Vector3(zombie.TargetPlayer.position.x, zombie.transform.position.y, zombie.TargetPlayer.position.z);
        zombie.transform.LookAt(targetPosition);

        if (Time.time >= zombie.LastAttackTime + zombie.AttackRate)
        {
            zombie.LastAttackTime = Time.time;
            zombie.Animator.SetTrigger("Attack");
        }
    }

    public override void Exit(ZombieFSM zombie)
    {
        // 다른 상태로 빠져나갈 때 정지 해제
        if (zombie.Agent.enabled) zombie.Agent.isStopped = false;
    }
}
