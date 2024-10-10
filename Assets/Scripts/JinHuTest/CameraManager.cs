using UnityEngine;

namespace DefaultNamespace
{
    public class CameraManager : MonoBehaviour
    {  
        private void Start()
        {
            // 隐藏鼠标
            Cursor.visible = false;   
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // 按下alt 显示鼠标，松开隐藏
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }


        #region 单例
        public static CameraManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("CameraManager").AddComponent<CameraManager>();
                } 
                return _instance;
            } 
        }

        private static CameraManager _instance; 
        private void Awake()
        {
            _instance = this;
        }
        #endregion 
    }
}