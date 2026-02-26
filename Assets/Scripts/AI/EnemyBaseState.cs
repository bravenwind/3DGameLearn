using UnityEngine;

// ==========================================
// 1. 상태 패턴의 기본이 되는 추상 클래스
// ==========================================
public abstract class EnemyBaseState
{
    // 상태 진입 시 1회 호출
    public abstract void Enter(ZombieFSM zombie);
    // 상태 머신 업데이트 (기존의 Update 역할)
    public abstract void Update(ZombieFSM zombie);
    // 상태를 빠져나갈 때 1회 호출
    public abstract void Exit(ZombieFSM zombie);
}
