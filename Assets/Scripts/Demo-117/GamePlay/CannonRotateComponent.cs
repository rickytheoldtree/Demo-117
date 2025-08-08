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
            if (GetMouseDown() && !IsOverUI())
            {
                mouseDown = true;
                lastMousePosition = GetMousePosition();
            }

            // 检测鼠标释放事件
            if (GetMouseUp())
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

        // 新增到你的类里
        private int activeFingerId = -1;   // -1 表示当前不使用触摸
        private Vector2 lastTouchPos;

        private Vector2 GetMousePosition()
        {
            if (activeFingerId != -1)
            {
                // 只读被锁定的手指
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var t = Input.touches[i];
                    if (t.fingerId == activeFingerId)
                    {
                        lastTouchPos = t.position;
                        return t.position;
                    }
                }
                // 找不到（本帧刚抬起等），先用上一次
                return lastTouchPos;
            }

            return Input.mousePosition;
        }

        private bool GetMouseDown()
        {
            // 有触摸：只认“第一个手指”——GetTouch(0)，并锁定它的 fingerId
            if (activeFingerId == -1 && Input.touchCount > 0)
            {
                var t0 = Input.GetTouch(0);
                activeFingerId = t0.fingerId;
                lastTouchPos = t0.position;
                return t0.phase == TouchPhase.Began;
            }

            // 否则用鼠标
            return Input.GetMouseButtonDown(0);
        }

        private bool GetMouseUp()
        {
            if (activeFingerId != -1)
            {
                // 只看被锁定的那根是否 Ended/Canceled
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var t = Input.touches[i];
                    if (t.fingerId == activeFingerId &&
                        (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
                    {
                        activeFingerId = -1;
                        return true;
                    }
                }

                // 若本帧没有任何触点，也算结束（某些平台抬起后 touchCount 先变 0）
                if (Input.touchCount == 0)
                {
                    activeFingerId = -1;
                    return true;
                }
                return false;
            }

            return Input.GetMouseButtonUp(0);
        }

        private bool IsOverUI()
        {
            if (activeFingerId != -1)
                return eventSystem.IsPointerOverGameObject(activeFingerId);

            return eventSystem.IsPointerOverGameObject();
        }
    }
}