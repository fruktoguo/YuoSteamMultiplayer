using System;
using UnityEngine;
using YuoTools;
using YuoTools.Extend.YuoXrTools;

namespace Game.Manager
{
    public class RTSCameraManager : MonoBehaviour
    {
        public YuoPoseSource move;
        public YuoPoseSource scale;

        OneEuroFilter<Vector3> filter;

        public Camera mainCamera;

        public Vector3 originalPosition;

        public Vector2 moveOffset;

        public float zoom;

        public float moveSpeed;
        public float zoomSpeed;

        public Vector3 targetPosition;

        private void Awake()
        {
            filter = new();
            mainCamera ??= Camera.main;
        }

        private void LateUpdate()
        {
            var moveDelta = move.ReadValue<Vector2>();
            var scaleDelta = scale.ReadValue<float>();

            targetPosition = originalPosition + moveDelta.x0y() * moveSpeed * Time.deltaTime;
            targetPosition += moveOffset.x0y();
            zoom += scaleDelta * zoomSpeed * Time.deltaTime;
            targetPosition += Vector3.up * zoom;
            
            mainCamera.transform.position = filter.Step(targetPosition, Time.deltaTime);
        }
    }
}