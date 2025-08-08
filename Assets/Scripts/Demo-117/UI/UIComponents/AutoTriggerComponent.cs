using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo_117.UI.UIComponents
{
    public class AutoTriggerComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float interval = 0.2f; // 自动触发事件的间隔时间
        private event Action OnAutoTrigger;
        private float lastTriggerTime; // 上次触发事件的时间
        private bool isMouseDown;
        
        private void OnDestroy()
        {
            // 确保在销毁时移除所有监听器
            OnAutoTrigger = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isMouseDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isMouseDown = false;
        }

        private void Update()
        {
            if (isMouseDown && Time.time - lastTriggerTime >= interval)
            {
                TriggerEvent();
                lastTriggerTime = Time.time;
            }
        }
        private void TriggerEvent()
        {
            OnAutoTrigger?.Invoke();
        }

        public void AddListener(Action onShootClick)
        {
            if (onShootClick != null)
            {
                OnAutoTrigger += onShootClick;
            }
        }
        
        public void RemoveListener(Action onShootClick)
        {
            if (onShootClick != null)
            {
                OnAutoTrigger -= onShootClick;
            }
        }
    }
}