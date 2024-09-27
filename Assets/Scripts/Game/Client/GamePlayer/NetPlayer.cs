using System;
using System.Collections.Generic;
using FishNet.Component.Animating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using SteamAPI.SteamHelper;
using TMPro;
using UnityEngine;
using YuoTools;

namespace Game.Client.GamePlayer
{ 
    /// <summary>
    /// 网络玩家控制脚本，负责处理玩家的移动、旋转以及动画。
    /// 继承自 NetworkBehaviour 以实现网络同步。
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetworkAnimator))]
    [RequireComponent(typeof(FootRotationHandler))] 
    public class NetPlayer : NetworkBehaviour
    {   
        readonly SyncVar<int> EatCount = new(0,new SyncTypeSettings(ReadPermission.ExcludeOwner)); 
        [Header("分数Text")] [SerializeField] private TextMeshProUGUI _ScoreText; 
        [Header("名称")] [SerializeField] private TextMeshProUGUI _NameText; 
        
        // 动画参数的哈希值，提升访问效率
        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int RotationStateHash = Animator.StringToHash("RotationSpeed");

        [Header("移动设置")] [SerializeField] private float moveSpeed = 5f; // 玩家移动速度

        [SerializeField] private float rotationSpeed = 720f; // 玩家旋转速度（度/秒）

        [SerializeField] private float inputThreshold = 0.05f; // 输入阈值，减少微小输入

        [Header("平滑设置")] [SerializeField] private float positionLerpSpeed = 10f; // 位置插值速度

        [SerializeField] private float rotationLerpSpeed = 10f; // 旋转插值速度

        [SerializeField] private float maxRotationPerFrame = 90f; // 每帧最大旋转角度

        [Header("网络同步设置")] [SerializeField] private bool useNetworkTransform = true; // 是否使用 NetworkTransform 进行位置和旋转同步

        [Header("角色模型")] public List<GameObject> playerModels;

        readonly SyncVar<int> playerModelType = new();

        private Rigidbody rb;
        private Animator animator;

        // 当前的输入方向
        private Vector3 inputDirection = Vector3.zero;

        // 旋转状态，同步变量，范围在-1到1之间
        private float rotationState = 0f;

        private void Awake()
        {
            // 获取必要的组件
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            // 确保 Rigidbody 不受重力影响（根据游戏需求调整）
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // 暂时先直接创建，之后可能是通过自己的netID去工厂获取。 
            AddArgsChangeEvent(); 
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner)
            {
                CameraManager.Instance.SetFollowTarget(transform);   // 先临时这么写
                
                var steamID = SteamAPIManager.Instance.GetMemberSteamId(OwnerId);
                if (steamID != default)
                { 
                    var friendPersonaName = SteamFriendsHelper.GetFriendPersonaName(steamID);
                    _NameText.text = friendPersonaName;
                }
            }
        } 

        private void AddArgsChangeEvent()
        {
            playerModelType.OnChange += OnModelTypeChange;
            EatCount.OnChange += OnEatCountChange;
        }
        
        private void RemoveArgsChangeEvent()
        {
            playerModelType.OnChange -= OnModelTypeChange;
            EatCount.OnChange -= OnEatCountChange;
        }

        void OnModelTypeChange(int prev, int next, bool asServer)
        {
            playerModels.HideAll();
            if (playerModels.Count > next)
            {
                playerModels[next].Show();
            }
        }
        
        void OnEatCountChange(int prev, int next, bool asServer)
        {
            // var steamID = SteamAPIManager.Instance.GetMemberSteamId(OwnerId);
            // if (steamID != default)
            // { 
            //     var friendPersonaName = SteamFriendsHelper.GetFriendPersonaName(steamID);
            //      
            // }    
            _ScoreText.text = next.ToString();
            Debug.Log($"[OwnerId:{OwnerId}]  --- EatCount:{next}");  
        }

        private void Update()
        {
            if (!IsOwner) return;

            HandleInput();
            UpdateAnimator();
            HandleModelInput();
        }

        void HandleModelInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SetPlayerModel((playerModelType.Value + 1) % playerModels.Count);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                SetPlayerModel((playerModelType.Value - 1 + playerModels.Count) % playerModels.Count);
            }
        }

        [ServerRpc]
        void SetPlayerModel(int index)
        {
            playerModelType.Value = index;
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            HandleMovement();
            HandleRotation();
        }

        /// <summary>
        /// 处理玩家的输入，包括移动方向和旋转状态。
        /// </summary>
        private void HandleInput()
        {
            // 获取水平和垂直输入（例如：键盘的A/D和W/S键）
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            // 创建一个标准化的移动方向向量
            Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;

            // 判断输入是否超过阈值
            if (direction.sqrMagnitude >= inputThreshold * inputThreshold)
            {
                inputDirection = direction;

                // 计算旋转状态
                CalculateRotationState(direction);
            }
            else
            {
                inputDirection = Vector3.zero;
                rotationState = 0f; // 没有输入时，不旋转
            }
        }

        /// <summary>
        /// 根据输入方向计算旋转状态（-1：左转，0：不转，1：右转）。
        /// 使用有符号角度确保180度时也能正确旋转，并映射到-1到1的范围内。
        /// </summary>
        /// <param name="direction">输入方向</param>
        private void CalculateRotationState(Vector3 direction)
        {
            Vector3 forward = rb.transform.forward;
            Vector3 inputDir = direction;

            // 计算玩家前向与输入方向之间的有符号角度
            float angle = Vector3.SignedAngle(forward, inputDir, Vector3.up);

            // 设置一个最小旋转阈值，例如10度，避免在角度变化过小时频繁切换旋转状态
            float minAngleThreshold = 10f;

            // 将角度映射到-1到1的范围内
            if (angle > minAngleThreshold)
            {
                rotationState = Mathf.Clamp(angle / 180f, 0f, 1f); // 右转
            }
            else if (angle < -minAngleThreshold)
            {
                rotationState = Mathf.Clamp(angle / 180f, -1f, 0f); // 左转
            }
            else
            {
                rotationState = 0f; // 不转
            }

            // 调试日志（可选）
            // Debug.Log($"Rotation State: {rotationState}, Angle: {angle}");
        }

        /// <summary>
        /// 处理玩家的移动。
        /// </summary>
        private void HandleMovement()
        {
            if (inputDirection != Vector3.zero)
            {
                // 计算移动距离
                Vector3 movement = inputDirection * moveSpeed * Time.fixedDeltaTime;
                Vector3 newPosition = rb.position + movement;

                // 使用平滑插值移动位置
                rb.MovePosition(Vector3.Lerp(rb.position, newPosition, positionLerpSpeed * Time.fixedDeltaTime));
            }
        }

        /// <summary>
        /// 处理玩家的旋转。
        /// </summary>
        private void HandleRotation()
        {
            if (rotationState == 0f)
            {
                // 不进行旋转
                return;
            }

            // 计算旋转方向和旋转速度
            float rotationDirection = Mathf.Sign(rotationState); // -1 或 1

            // 计算旋转量，并限制最大旋转角度
            float rotationAmount =
                Mathf.Clamp(Mathf.Abs(rotationState) * rotationSpeed * Time.fixedDeltaTime * rotationDirection,
                    -maxRotationPerFrame, maxRotationPerFrame);

            // 计算新的旋转
            Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
            Quaternion newRotation = rb.rotation * deltaRotation;

            // 应用旋转，使用 RotateTowards 保证旋转方向的一致性
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        /// <summary>
        /// 更新动画参数，根据移动速度和旋转状态。
        /// 移动速度被规范化到0-1范围内。
        /// 旋转状态是一个平滑的值，在-1到1之间。
        /// </summary>
        private void UpdateAnimator()
        {
            // 计算当前移动速度
            float currentSpeed = inputDirection.magnitude * moveSpeed;

            // 规范化移动速度到0-1范围
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / moveSpeed);

            // 平滑移动速度参数
            float smoothedSpeed = Mathf.Lerp(animator.GetFloat(MoveSpeedHash), normalizedSpeed, Time.deltaTime * 10f);
            animator.SetFloat(MoveSpeedHash, smoothedSpeed);

            // 设置旋转状态参数，确保其在-1到1之间
            float clampedRotationState = Mathf.Clamp(rotationState, -1f, 1f);
            animator.SetFloat(RotationStateHash, clampedRotationState);
        }


        private void OnCollisionEnter(Collision other)
        {
            if (!IsOwner) return;
            {
                if (other.gameObject.CompareTag($"Enemy_Cube"))
                {
                    S_OnCollisionServer(other.gameObject); 
                }
            }
        }

        [ServerRpc]
        private void S_OnCollisionServer(GameObject other)
        {
            EatCount.Value++;
            Destroy(other);
        }

        private void OnDestroy()
        {
            RemoveArgsChangeEvent(); 
        }
    }

}