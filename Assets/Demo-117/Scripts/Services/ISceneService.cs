using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RicKit.RFramework;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Demo_117.Services
{
    public interface ISceneService : IService
    {
        void LoadScene(string sceneName, LoadSceneMode mode);
        void UnloadScene(string sceneName);
        UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode);
    }
    
    public class SceneService : AbstractService, ISceneService
    {
        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            SceneManager.LoadScene(sceneName, mode);
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        public UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            // 异步加载场景
            return SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask();
        }
    }
    
    public class AddressablesSceneService : AbstractService, ISceneService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            var handle = Addressables.LoadSceneAsync(sceneName, mode);
            loadedScenes[sceneName] = handle;
            handle.WaitForCompletion();
        }

        public void UnloadScene(string sceneName)
        {
            if (!loadedScenes.TryGetValue(sceneName, out var handle))
            {
                SceneManager.UnloadSceneAsync(sceneName);
                return;
            }
            Addressables.UnloadSceneAsync(handle);
            loadedScenes.Remove(sceneName);
        }

        public UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            var handle = Addressables.LoadSceneAsync(sceneName, mode);
            loadedScenes[sceneName] = handle;
            return handle.ToUniTask();
        }
    }
}