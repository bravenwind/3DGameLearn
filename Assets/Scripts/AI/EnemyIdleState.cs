using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float idleTimer;

    public override void Enter(ZombieFSM zombie)
    {
        idleTimer = 0.0f;
        zombie.Agent.ResetPath();
        zombie.Animator.SetBool("IsMoving", false);
    }

    public override void Update(ZombieFSM zombie)
    {
        // 1. 상태 전환(Transition) 조건 검사
        if (zombie.TargetPlayer != null)
        {
            float distance = Vector3.Distance(zombie.transform.position, zombie.TargetPlayer.position);
            if (zombie.DetectPlayer_Sight(distance)) { zombie.ChangeState(zombie.StateChase); return; }
            if (zombie.DetectPlayer_Audio(distance)) { zombie.ChangeState(zombie.StateSuspicious); return; }
        }

        // 2. 대기 행동(Execute)
        idleTimer += Time.deltaTime;
        if (idleTimer >= zombie.IdleDuration)
        {
            zombie.ChangeState(zombie.StatePatrol);
        }
    }

    public override void Exit(ZombieFSM zombie) { }
}
