using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform leftHandGrip;
    [SerializeField] private Transform rightHandGrip;

    private void OnAnimatorIK(int layerIndex)
    {
        if (leftHandGrip == null || rightHandGrip == null)
        {
            return;
        }

        // 왼손의 위치와 회전을 타겟에 맞추라고 명령
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandGrip.position);

        // 회전 (손목 꺾임) 맞추기
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandGrip.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandGrip.position);

        // 회전 (손목 꺾임) 맞추기
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandGrip.rotation);
    }
}
