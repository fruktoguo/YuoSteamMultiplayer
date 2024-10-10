using System;
using FishNet.Object;
using UnityEngine;

public class PlayerController: NetworkBehaviour
{
    public CharacterController controller;
    public float speed = 3.0f;
    public float turnSpeed = 400.0f;
    public Animator animator;
    public Camera _camera;

    private Transform selectTransform;
    private Transform talkTransform;
    
     
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
    
        // 获取相机相对方向
        if (_camera != null)
        {
            if (talkTransform != null)
            {
                // 始终朝向目标
                Vector3 targetDir = talkTransform.position - transform.position;
                targetDir.y = 0;
                float step = 20 * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                var targetRotation = Quaternion.LookRotation(newDir); 
                transform.rotation = targetRotation;
                return;
            }
            
            
            
            Vector3 forward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 right = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;
    
            // 设定移动向量为按摄像机方向的左右与上下
            Vector3 move = moveVertical * forward + moveHorizontal * right; 
            controller.SimpleMove(move * speed); 
            if (move != Vector3.zero) 
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }

            if (selectTransform != null)
            {
                // 始终朝向目标
                Vector3 targetDir = selectTransform.position - transform.position;
                float step = 20 * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                var targetRotation = Quaternion.LookRotation(newDir);
                targetRotation.x = 0; // 保持y轴不变
                transform.rotation = targetRotation;
            }
        }
    }
    
    public void SetSelectTarget(Transform target)
    {
        selectTransform = target;
    }

    /// <summary>
    /// 设置对话目标 
    /// </summary>
    public void SetTalkTarget(Transform target)
    {
        talkTransform = target;
    }
    
    
}