using FishNet.Object;
using UnityEngine;

namespace DefaultNamespace
{
    public class NetPlayer : NetworkBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float moveSpeed = 5f; // 玩家移动速度
        [SerializeField] private float rotateSpeed = 720f; // 玩家旋转速度（度/秒）
        [SerializeField] private float teleportCooldown = 5f; // 传送冷却时间（秒）
        [SerializeField] private float inputThreshold = 0.1f; // 输入阈值，减少微小输入
        [SerializeField] private float teleportCheckRadius = 0.5f; // 传送位置周围检查半径
        [SerializeField] private LayerMask teleportObstacleLayers; // 传送障碍物图层
        private Rigidbody rb;
        // 输入缓冲区
        private Vector3 inputMoveDirection = Vector3.zero;
        private float inputRotateAmount;
        // 预先分配的碰撞体数组，避免每次调用时分配新的数组
        private Collider[] overlapResults = new Collider[10]; // 根据需求调整大小
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>(); 
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (NetworkManager.ClientManager.Started)
                {
                    // 客户端已经启动，执行操作
                    TestSend();
                }
                else
                {
                    Debug.LogWarning("Client is not active yet.");
                } 
            }
            
            if (!IsOwner) return;

            HandleInput();
        }


        [ServerRpc(RequireOwnership = false)]
        private void TestSend()
        {
            Debug.Log("TestSend");
        }
        
        
        private void FixedUpdate()
        {
            if (!IsOwner) return;

            // 发送移动和旋转数据的固定间隔
            if (inputMoveDirection.sqrMagnitude > 0 || Mathf.Abs(inputRotateAmount) > 0)
            {
                SubmitMovementAndRotation(inputMoveDirection, inputRotateAmount);
                // 重置输入缓冲区
                inputMoveDirection = Vector3.zero;
                inputRotateAmount = 0f;
            }
        }
        
        /// <summary>
        /// 处理玩家的输入，包括移动、旋转和传送。
        /// 将输入存储在缓冲区中，以便在 FixedUpdate 中统一发送。
        /// </summary>
        private void HandleInput()
        {
            // 获取水平和垂直输入（例如：键盘的A/D和W/S键）
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            // 创建一个标准化的移动方向向量
            Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

            if (moveDirection.sqrMagnitude >= inputThreshold * inputThreshold)
            {
                // 不在客户端进行缩放，留给服务器处理
                inputMoveDirection += moveDirection;
            } 
        } 
        
        /// <summary>
        /// 服务器远程过程调用（ServerRpc），接收客户端的移动和旋转请求，并更新 Rigidbody。
        /// </summary>
        /// <param name="move">移动向量（未缩放）</param>
        /// <param name="rotate">旋转量（未缩放）</param>
        // [ServerRpc(RequireOwnership = true)]
        // private void SubmitMovementAndRotationServerRpc(Vector3 move, float rotate)
        private void SubmitMovementAndRotation(Vector3 move, float rotate)
        {
            // 安全性验证：限制移动和旋转的最大值
            float maxMovePerFrame = moveSpeed * Time.fixedDeltaTime * 2; // 允许一定的缓冲
            float maxRotatePerFrame = rotateSpeed * Time.fixedDeltaTime * 2;

            // 计算实际移动量
            Vector3 actualMove = move * moveSpeed * Time.fixedDeltaTime;

            if (actualMove.magnitude > maxMovePerFrame)
            {
                actualMove = actualMove.normalized * maxMovePerFrame;
                Debug.LogWarning($"玩家{OwnerId}尝试超出移动限制，已调整移动量。");
            }

            // 计算实际旋转量
            float actualRotate = rotate * rotateSpeed * Time.fixedDeltaTime;

            if (Mathf.Abs(actualRotate) > maxRotatePerFrame)
            {
                actualRotate = Mathf.Sign(actualRotate) * maxRotatePerFrame;
                Debug.LogWarning($"玩家{OwnerId}尝试超出旋转限制，已调整旋转量。");
            }

            // 使用 Rigidbody 移动和旋转
            rb.MovePosition(rb.position + actualMove);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, actualRotate, 0));
        }
        
    }
}