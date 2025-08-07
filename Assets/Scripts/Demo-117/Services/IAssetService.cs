using System;
using Cysharp.Threading.Tasks;
using RicKit.RFramework;
using RicKit.UI.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo_117.Services
{
    public interface IAssetService : IService, IPanelLoader //同时实现UIManager需要的加载界面服务
    {
        T Load<T>(string key) where T : Object;
        bool TryLoad<T>(string key, out T asset) where T : Object;
    }
    //用Resources举例
    public class ResourcesAssetService : AbstractService, IAssetService
    {
        public async UniTask<GameObject> LoadPrefabAsync(string path)
        {
            var req = Resources.LoadAsync<GameObject>(path);
            await UniTask.WaitUntil(() => req.isDone);
            return req.asset as GameObject;
        }

        public GameObject LoadPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public T Load<T>(string key) where T : Object
        {
            return Resources.Load<T>(key);
        }

        public bool TryLoad<T>(string key, out T asset) where T : Object
        {
            try
            {
                asset = Resources.Load<T>(key);
                return asset;
            }
            catch
            {
                asset = null;
                return false;
            }
        }
    }
}