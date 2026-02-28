using System;
using RicKit.RFramework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo_117.Services
{
    // 该服务用于自动保存游戏状态
    // 例如在应用程序暂停或失去焦点时触发保存
    // 通过注册和注销回调函数来控制保存行为
    public interface IAutoSaveService : IService
    {
        void RegisterAutoSave(Action action);
        void UnregisterAutoSave(Action action);
    }
    public class AutoSaveService : AbstractService, IAutoSaveService
    {
        private AutoSave mono;
        public override void Init()
        {
            mono = new GameObject("AutoSave").AddComponent<AutoSave>();
            Object.DontDestroyOnLoad(mono.gameObject);
        }

        public void RegisterAutoSave(Action action)
        {
            mono.OnAutoSave += action;
        }

        public void UnregisterAutoSave(Action action)
        {
            mono.OnAutoSave -= action;
        }
    }
    public class AutoSave : MonoBehaviour
    {
        public event Action OnAutoSave;
        private bool savedThisFrame;

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus || savedThisFrame)
            {
                savedThisFrame = false;
                return;
            }
            OnAutoSave?.Invoke();
            savedThisFrame = true;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus || savedThisFrame)
            {
                savedThisFrame = false;
                return;
            }
            OnAutoSave?.Invoke();
            savedThisFrame = true;
        }
    }
}