using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void Enter(ZombieFSM zombie)
    {
        zombie.Die();
    }

    public override void Update(ZombieFSM zombie) { }
    public override void Exit(ZombieFSM zombie) { }
}
