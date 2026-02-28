using Demo_117.Services;
using RicKit.RFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo_117.GamePlay
{
    public struct RotateCannonEvent
    {
        public Vector2 delta;
        public bool reverseY;
    }
    public class InputAreaComponent : MonoBehaviour, ICanGetLocator<Entity>, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        private int activeFingerId = int.MinValue;   // int.MinValue 表示当前没有激活的手指ID
        private bool reverseY;
        
        private IGameCameraService gameCameraService;
        
        private void Awake()
        {
            // 获取EventSystem和服务
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
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if(activeFingerId != int.MinValue) return;
            activeFingerId = eventData.pointerId;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != activeFingerId) return;
            activeFingerId = int.MinValue; 
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (eventData.pointerId != activeFingerId) return;
            this.SendEvent(new RotateCannonEvent
            {
                delta = eventData.delta,
                reverseY = reverseY
            });
        }
    }
}