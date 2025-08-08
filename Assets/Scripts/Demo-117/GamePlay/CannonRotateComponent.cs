using System;
using Demo_117.Services;
using RicKit.RFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo_117.GamePlay
{
    public class CannonRotateComponent : MonoBehaviour, ICanGetLocator<Entity>
    {
        private EventSystem eventSystem;
        
        private Vector2 lastMousePosition;
        private bool mouseDown;
        private bool reverseY;
        
        private IGameCameraService gameCameraService;
        
        private void Awake()
        {
            // 获取EventSystem和服务
            eventSystem = EventSystem.current;
            this.TryGetService(out gameCameraService);
        }

        private void OnEnable()
        {
            // 注册摄像机变更事件
            this.RegisterEvent<CameraChangeEvent>(OnCameraChanged);
            reverseY = gameCameraService.GetCurrentCameraData().reverseY;
        }
        
        private void OnDisable()
        {
            // 注销摄像机变更事件
            this.UnRegisterEvent<CameraChangeEvent>(OnCameraChanged);
        }
        
        private void OnCameraChanged(CameraChangeEvent e)
        {
            reverseY = e.data.reverseY;
        }

        private void Update()
        {
            // 检测鼠标按下事件
            if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject())
            {
                mouseDown = true;
                lastMousePosition = GetMousePosition();
            }

            // 检测鼠标释放事件
            if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
            }

            // 如果鼠标按下且未在UI上，则旋转炮台
            if (mouseDown)
            {
                Vector2 delta = GetMousePosition() - lastMousePosition;
                RotateCannon(delta);
                lastMousePosition = GetMousePosition();
            }
        }
        
        private void RotateCannon(Vector2 delta)
        {
            float rotationSpeed = 0.1f; // 旋转速度

            // 当前欧拉角
            Vector3 euler = transform.localEulerAngles;

            // 转成 -180 ~ 180
            if (euler.x > 180f) euler.x -= 360f;
            if (euler.y > 180f) euler.y -= 360f;

            // 增加旋转量
            euler.x = reverseY ? euler.x - delta.y * rotationSpeed : euler.x + delta.y * rotationSpeed; // 注意鼠标Y要反向
            euler.y += delta.x * rotationSpeed;

            // 限制范围
            euler.x = Mathf.Clamp(euler.x, 15f, 60f); // X轴范围
            euler.y = Mathf.Clamp(euler.y, -45f, 45f); // Y轴范围

            // 应用回去
            transform.localEulerAngles = euler;
        }


        // 针对不同平台获取鼠标位置
        private Vector2 GetMousePosition()
        {
            return Application.isEditor ?
                new Vector2(Input.mousePosition.x, Input.mousePosition.y) :
                new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
        }
    }
}