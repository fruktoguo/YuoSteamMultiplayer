using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Client.GamePlayer
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Cinemachine Components")]
        public CinemachineTargetGroup targetGroup;
        public CinemachineOrbitalFollow freeLook;
        public CinemachineInputAxisController inputController;
        public CinemachineCamera cCamera;
        public Camera MainCamera;
        
        [Header("Camera Settings")]
        [SerializeField] private float offsetDistance = 3f;
        [SerializeField] private float focusAnimTime = 0.25f;
        [SerializeField] private float transitionDuration = 0.35f;

        // [Header("Test Objects")]
        public Transform selectTarget;
        public Transform talkTarget;
        // public PlayerController playerController;   // 暂时不用这个，回头有需求再写到netPlayer里面

        private PlayerCameraState _currentState;
        private Vector3[] _lookAtPath;
        private Sequence _currentAnimation;
        private bool _isSelectTarget;
        private bool _fixedFollowPlayer;  

        private void Awake()
        {
            DOTween.Init();
            SetState(new NormalState(this));
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        public void SetOwnerCamera(bool isOwner)
        { 
            gameObject.SetActive(isOwner);
        }
        
        private void Update()
        {
            HandleInput();
            _currentState.Update();
        }

        private void HandleInput()
        {
            // if (Input.GetKeyDown(KeyCode.R)) ResetToPlayerBack(playerController.transform, transitionDuration);
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!selectTarget)
                {
                    return;
                }
                ToggleTargetSelection();
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!talkTarget)
                {
                    return;
                }
                SetState(new TalkState(this));
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!talkTarget)
                {
                    return;
                }
                EndTalkState();
            }
        }

        public void SetState(PlayerCameraState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        /// <summary>
        /// 回到玩家背后   x轴需要和当前玩家的旋转角度一致
        /// </summary> 
        public void ResetToPlayerBack(Transform playerTran, float duration = 0.5f, Action callback = null)
        { 
            // 首先，停止上一个动画（如果存在）
             _currentAnimation?.Kill();

             float currentAngle = freeLook.HorizontalAxis.Value; 
             float targetAngle = playerTran.eulerAngles.y;
             float anglesX = Mathf.DeltaAngle(currentAngle, targetAngle);

             // 创建新的动画序列
             _currentAnimation = DOTween.Sequence()
                 .Insert(0, DOTween.To(() => freeLook.HorizontalAxis.Value, x => freeLook.HorizontalAxis.Value = x, currentAngle + anglesX, duration).SetEase(Ease.Linear))
                 .Insert(0, DOTween.To(() => freeLook.VerticalAxis.Value, y => freeLook.VerticalAxis.Value = y, 0.5f, duration).SetEase(Ease.Linear));
             _currentAnimation.OnComplete(() =>
             {
                 callback?.Invoke();
             });
             // 开始播放新的动画
             _currentAnimation.Play();
        }

         /// <summary>
         /// 设置旋转角度 从当前位置 -> 看向目标的位置
         /// </summary>
        public void LookAtTargetToPlayerBack(Transform playerTran, Transform targetTran, float duration = 0.5f, Action callback = null)
        {
            // Implementation remains the same
            var curDir = targetTran.position - playerTran.position; 
             float currentAngleX = freeLook.HorizontalAxis.Value; 
             float targetAngleX = Quaternion.LookRotation(curDir).eulerAngles.y;
             float anglesX = Mathf.DeltaAngle(currentAngleX, targetAngleX);
             
                // 首先，停止上一个动画（如果存在）
             _currentAnimation?.Kill();
             
             if (duration == 0)
             {
                freeLook.HorizontalAxis.Value = currentAngleX + anglesX;
                freeLook.VerticalAxis.Value = 30f;
                callback?.Invoke();
                return; 
             } 
             
             // 创建新的动画序列
             _currentAnimation = DOTween.Sequence()
                 .Insert(0,
                     DOTween.To(() => freeLook.HorizontalAxis.Value, x => freeLook.HorizontalAxis.Value = x, currentAngleX + anglesX,
                         duration).SetEase(Ease.Linear))
                 .Insert(0,
                     DOTween.To(() => freeLook.VerticalAxis.Value, y => freeLook.VerticalAxis.Value = y, 30f, duration)
                         .SetEase(Ease.Linear));
             _currentAnimation.OnComplete(() =>
             {
                 callback?.Invoke();
             });
             // 开始播放新的动画
             _currentAnimation.Play(); 
        }

        public void FocusTargetAnim(bool isIn, Action callback = null)
        {
            if (isIn)
            {
             var focusTargetAnim = DOTween.Sequence()
                 .Insert(0,
                     DOTween.To(() => cCamera.Lens.FieldOfView, fov => cCamera.Lens.FieldOfView = fov,
                         45f,
                         focusAnimTime));
             focusTargetAnim.OnComplete(() =>
             {
                 Debug.Log("缩放成功！");
                 callback?.Invoke();
             }); 
             focusTargetAnim.Play();
             Debug.Log("开始对话，播放开始动画");
            }
            else
            { 
             var focusTargetAnim = DOTween.Sequence()
                 .Insert(0,
                     DOTween.To(() => cCamera.Lens.FieldOfView, fov => cCamera.Lens.FieldOfView = fov,
                         60f,
                         focusAnimTime)); 
             focusTargetAnim.OnComplete(() =>
             {
                 Debug.Log("结束对话，播放结束动画");
                 callback?.Invoke();
             }); 
             focusTargetAnim.Play(); 
            }
        }

        /// <summary>
        /// 选择看多个目标的目标  (Tab 锁定目标的功能)
        /// </summary> 
        public void SelectLookTarget(Transform target,float weight = 5,float weightTime = 0.35f)
        {
            // 回去玩家背后
            if (target == null)
            {
                if (targetGroup.Targets.Count == 2)
                {
                    targetGroup.Targets[0].Weight = 10;
                    targetGroup.Targets.RemoveAt(1); 
                }
                _fixedFollowPlayer = false;
                return;
            } 
            if (targetGroup.Targets.Count == 1)
            {
                targetGroup.AddMember(target,0,1);
                targetGroup.Targets[0].Weight = 5;
                // tween控制权重
                DOTween.To(() => targetGroup.Targets[1].Weight, x => targetGroup.Targets[1].Weight = x, weight,
                 weightTime);
            }
            else
            {
                targetGroup.Targets[1].Object = target;
            }
        }
         
        /// <summary>
        /// 将相机移动到两个角色的侧面，同时看向两个角色
        /// </summary> 
        public void LookAtBothTargetsSide(Vector3[] path, float duration = 0.5f, Action callBack = null)
        {  
             // 创建动画序列
             var newAnimation = DOTween.Sequence()
                 .Append(freeLook.transform.DOPath(path, duration)); //SetEase(Ease.InOutSine)
             newAnimation.OnComplete(() =>
             {
                 callBack?.Invoke();
             }); 
             newAnimation.Play();
        }

        public Vector3[] GetLookAtBothTargetPath(Transform playerTran, Transform targetTran)
        {   
             // 计算主角到NPC的方向
             Vector3 playerToTarget = targetTran.position - playerTran.position;
             Vector3 sideDirectionVector = Vector3.Cross(Vector3.up, playerToTarget).normalized;

             // 判断相机当前在主角的左侧还是右侧
             float sideDirection = Vector3.Dot(freeLook.transform.position - playerTran.position, sideDirectionVector) > 0 ? 1 : -1;

             // 计算目标位置，在主角位置向左或向右移动一定距离
             offsetDistance = 3f; // 可以调整这个值来改变相机偏移的距离
             Vector3 targetPosition = playerTran.position + sideDirectionVector * sideDirection * offsetDistance;
             targetPosition.y = freeLook.transform.position.y; // 保持y轴不变

             // 创建路径点
             Vector3 start = freeLook.transform.position;
             Vector3 end = targetPosition;

             // 创建贝塞尔曲线的控制点
             Vector3 controlPoint = (start + end) / 2 + sideDirectionVector * offsetDistance * 0.5f;

             // 创建路径点
             Vector3[] path = new Vector3[3];
             path[0] = start;
             path[1] = controlPoint;
             path[2] = end;

             return path;
        } 
        
        private void ToggleTargetSelection()
        {
            _isSelectTarget = !_isSelectTarget;
            SetState(_isSelectTarget ? new LockTargetState(this) : new NormalState(this));
        }

        private void EndTalkState()
        {
            (_lookAtPath[0], _lookAtPath[2]) = (_lookAtPath[2], _lookAtPath[0]);
            LookAtBothTargetsSide(_lookAtPath, transitionDuration);
            // playerController.SetTalkTarget(null);
            SelectLookTarget(null);
            FocusTargetAnim(false, () => SetState(new NormalState(this)));
        }
 
        public bool FixedFollowPlayer
        {
            get => _fixedFollowPlayer;
            set => _fixedFollowPlayer = value;
        }
        public Transform SelectTarget => selectTarget;
        public Transform TalkTarget => talkTarget;
        // public PlayerController PlayerController => playerController;
        public Vector3[] LookAtPath { get => _lookAtPath; set => _lookAtPath = value; }
    }

    public abstract class PlayerCameraState
    {
        protected PlayerCamera Camera;

        protected PlayerCameraState(PlayerCamera camera)
        {
            Camera = camera;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }

    public class NormalState : PlayerCameraState
    {
        public NormalState(PlayerCamera camera) : base(camera) { }

        public override void Enter()
        {
            Camera.inputController.enabled = true;
            Camera.freeLook.enabled = true;
            Camera.SelectLookTarget(null);
            // Camera.PlayerController.SetSelectTarget(null);
            // Camera.PlayerController.SetTalkTarget(null);
        }

        public override void Update() { }
        public override void Exit() { }
    }

    public class LockTargetState : PlayerCameraState
    {
        public LockTargetState(PlayerCamera camera) : base(camera) { }

        public override void Enter()
        {
            Camera.inputController.enabled = false;
            if (Camera.SelectTarget != null && IsTargetVisible())
            {
                // Camera.LookAtTargetToPlayerBack(Camera.PlayerController.transform, Camera.SelectTarget, 0.15f, () => Camera.FixedFollowPlayer = true);
                Camera.SelectLookTarget(Camera.SelectTarget,1);
                // Camera.PlayerController.SetSelectTarget(Camera.SelectTarget);
            }
            else
            {
                Camera.SetState(new NormalState(Camera));
            }
        }

        public override void Update()
        {
            if (Camera.FixedFollowPlayer && Camera.SelectTarget != null)
            {
                // Camera.LookAtTargetToPlayerBack(Camera.PlayerController.transform, Camera.SelectTarget, 0);
            }
        }

        public override void Exit() { }

        private bool IsTargetVisible()
        {
            Vector3 screenPos = Camera.MainCamera.WorldToScreenPoint(Camera.SelectTarget.position);
            return screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height;
        }
    }

    public class TalkState : PlayerCameraState
    {
        public TalkState(PlayerCamera camera) : base(camera) { }

        public override void Enter()
        {
            Camera.inputController.enabled = false;
            Camera.freeLook.enabled = false;
            if (Camera.TalkTarget != null)
            {
                Camera.FocusTargetAnim(true);
                // Camera.LookAtPath = Camera.GetLookAtBothTargetPath(Camera.PlayerController.transform, Camera.TalkTarget);
                Camera.LookAtBothTargetsSide(Camera.LookAtPath, 0.35f);
                // Camera.PlayerController.SetTalkTarget(Camera.TalkTarget);
                Camera.SelectLookTarget(Camera.TalkTarget, 10f, 0);
            }
        }

        public override void Update() { }
        public override void Exit() { }
    }
}