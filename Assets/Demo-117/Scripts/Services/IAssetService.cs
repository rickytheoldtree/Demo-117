using Cysharp.Threading.Tasks;
using RicKit.RFramework;
using RicKit.UI.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Demo_117.Services
{
    public interface IAssetService : IService, IPanelLoader //同时实现UIManager需要的加载界面服务
    {
        T Load<T>(string key) where T : Object;
        bool TryLoad<T>(string key, out T asset) where T : Object;
        UniTask<T> LoadAssetAsync<T>(string key) where T : Object;
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

        public UniTask<T> LoadAssetAsync<T>(string key) where T : Object
        {
            var req = Resources.LoadAsync<T>(key);
            return UniTask.WaitUntil(() => req.isDone).ContinueWith(() => req.asset as T);
        }
    }

    //用Addressables举例
    public class AddressablesAssetService : AbstractService, IAssetService
    {
        public UniTask<GameObject> LoadPrefabAsync(string path)
        {
            return Addressables.LoadAssetAsync<GameObject>($"Assets/Demo-117/UI/{path}.prefab").ToUniTask();
        }

        public GameObject LoadPrefab(string path)
        {
            return Addressables.LoadAssetAsync<GameObject>($"Assets/Demo-117/UI/{path}.prefab").WaitForCompletion();
        }

        public T Load<T>(string key) where T : Object
        {
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        }

        public bool TryLoad<T>(string key, out T asset) where T : Object
        {
            try
            {
                asset = Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
                return asset;
            }
            catch
            {
                asset = null;
                return false;
            }
        }

        public UniTask<T> LoadAssetAsync<T>(string key) where T : Object
        {
            return Addressables.LoadAssetAsync<T>(key).ToUniTask();
        }
    }
}