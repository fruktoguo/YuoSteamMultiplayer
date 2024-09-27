using System;
using Unity.Cinemachine;
using UnityEngine;
using YuoTools;

public class CameraManager : SingletonMono<CameraManager>
{   
    public CinemachineCamera playerFollowCamera;

    private void Start()
    {
        playerFollowCamera = transform.Find("PlayerFollowCamera").GetComponent<CinemachineCamera>();
    }
    

    public void SetFollowTarget(Transform target)
    {
        playerFollowCamera.Follow = target;
        playerFollowCamera.LookAt = target;
    }
}
