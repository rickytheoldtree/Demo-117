using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Demo_117.UI.UIComponents
{
    [RequireComponent(typeof(RectTransform))]
    public class AutoTriggerComponent : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        [Header("Repeat Settings")]
        public float interval = 0.2f;                // 后续重复间隔

        [Header("Raycast Pass-through")]
        public bool passRaycast = true;              // 是否转发 Pointer 事件给下层
        public bool passOnlyNext = true;             // 只转发给第一个下层目标（而不是所有）

        private event Action OnAutoTrigger;

        private bool isPressed;
        private Coroutine repeatCo;

        // 缓存：所在 Canvas 的 GraphicRaycaster（UI）和场景 EventSystem
        private GraphicRaycaster graphicRaycaster;
        private EventSystem eventSystem;

        private void Awake()
        {
            eventSystem = EventSystem.current;
            var canvas = GetComponentInParent<Canvas>();
            if (canvas) graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }

        private void OnDisable()
        {
            StopRepeating();
        }

        private void OnDestroy()
        {
            // 防止外部未移除的委托导致泄漏
            OnAutoTrigger = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            StartRepeating();

            // 事件穿透（把这次 PointerDown 继续发给下层）
            if (passRaycast)
                PassPointer(eventData, ExecuteEvents.pointerDownHandler);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopRepeating();
            isPressed = false;

            // 事件穿透（把这次 PointerUp 继续发给下层）
            if (passRaycast)
                PassPointer(eventData, ExecuteEvents.pointerUpHandler);
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            if (!isPressed) return;
            if (passRaycast)
            {
                PassPointer(eventData, ExecuteEvents.pointerMoveHandler);
            }

        }

        private void StartRepeating()
        {
            if (repeatCo != null) return;
            repeatCo = StartCoroutine(CoRepeat());
        }

        private void StopRepeating()
        {
            if (repeatCo != null)
            {
                StopCoroutine(repeatCo);
                repeatCo = null;
            }
            isPressed = false;
        }

        private IEnumerator CoRepeat()
        {
            while (isPressed)
            {
                OnAutoTrigger?.Invoke();
                yield return new WaitForSeconds(interval);
            }
        }

        public void AddListener(Action onTrigger)
        {
            if (onTrigger != null)
                OnAutoTrigger += onTrigger;
        }

        public void RemoveListener(Action onTrigger)
        {
            if (onTrigger != null)
                OnAutoTrigger -= onTrigger;
        }

        /// <summary>
        /// 将当前 Pointer 事件转发给“下一个”命中的对象（UI 或 3D）。
        /// </summary>
        private void PassPointer<T>(PointerEventData original, ExecuteEvents.EventFunction<T> handler)
            where T : IEventSystemHandler
        {
            if (eventSystem == null) return;

            // 1) 先用 UI GraphicRaycaster 找下层 UI
            if (graphicRaycaster)
            {
                var results = RaycastResultsUI;
                results.Clear();

                var tempData = new PointerEventData(eventSystem)
                {
                    position = original.position,
                    button = original.button,
                    pointerId = original.pointerId,
                    clickCount = original.clickCount,
                    pressPosition = original.pressPosition
                };

                graphicRaycaster.Raycast(tempData, results);

                foreach (var result in results)
                {
                    var go = result.gameObject;
                    if (!go) continue;
                    if (go == gameObject || go.transform.IsChildOf(transform)) continue; // 跳过自身与子层
                    if (!ExecuteEvents.CanHandleEvent<T>(go)) continue;

                    ExecuteEvents.Execute(go, original, handler);
                    if (passOnlyNext) break;
                }
            }
        }

        // 复用列表以减少 GC
        private static readonly List<RaycastResult> RaycastResultsUI = new List<RaycastResult>(16);
    }
}