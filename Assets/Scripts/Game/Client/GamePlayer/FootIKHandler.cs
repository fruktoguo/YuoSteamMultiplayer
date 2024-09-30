using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootRotationHandler : NetworkBehaviour
{
    private Animator animator;

    [Header("IK Settings")]
    [SerializeField]
    private float footIKWeight = 1f; // IK权重

    [SerializeField]
    private float footRotationWeight = 1f; // 脚部旋转的IK权重

    [SerializeField]
    private float footRayDistance = 1.5f; // 脚部射线检测距离

    [SerializeField]
    private LayerMask groundLayer; // 地面的Layer

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!IsOwner) return; // 仅由拥有者处理IK

        if (animator)
        {
            // 处理左脚
            AdjustFootRotation(AvatarIKGoal.LeftFoot);

            // 处理右脚
            AdjustFootRotation(AvatarIKGoal.RightFoot);
        }
    }

    /// <summary>
    /// 调整指定脚的IK旋转，使脚底板与地面平行。
    /// </summary>
    /// <param name="goal">要调整的脚</param>
    private void AdjustFootRotation(AvatarIKGoal goal)
    {
        // 获取当前脚的位置
        Vector3 footPosition = animator.GetIKPosition(goal);

        // 发射射线检测地面
        var height = 0.5f;
        Vector3 rayOrigin = footPosition + Vector3.up * height; // 从脚上方发射射线

        if (Physics.Raycast(rayOrigin, Vector3.down, out var hit, footRayDistance + height, groundLayer))
        {
            // 获取地面法线
            Vector3 groundNormal = hit.normal;

            // 计算脚的目标旋转，使脚底板与地面法线平行
            // 这里假设角色的前方与动画中的脚部前方向一致
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, groundNormal) * transform.rotation;

            // 设置IK旋转权重
            animator.SetIKRotationWeight(goal, footRotationWeight);

            // 应用旋转
            animator.SetIKRotation(goal, targetRotation);
        }
        else
        {
            // 如果未检测到地面，则保持原有旋转
            animator.SetIKRotationWeight(goal, 0f);
        }
    }
}