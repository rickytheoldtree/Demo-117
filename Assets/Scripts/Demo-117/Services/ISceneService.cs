using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RicKit.RFramework;
using UnityEngine.SceneManagement;

namespace Demo_117.Services
{
    public interface ISceneService : IService
    {
        void LoadScene(string sceneName);
        void UnloadScene(string sceneName);
        UniTask LoadSceneAsync(string game);
    }
    
    public class SceneService : AbstractService, ISceneService
    {
        public void LoadScene(string sceneName)
        {
            // 示例代码，实际实现可能需要使用 UnityEngine.SceneManagement.SceneManager
            // 或者 Addressables 来加载场景
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            // 或者使用 Addressables
            // Addressables.LoadSceneAsync(sceneName);
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        public UniTask LoadSceneAsync(string game)
        {
            // 异步加载场景
            return SceneManager.LoadSceneAsync(game, LoadSceneMode.Additive).ToUniTask();
        }
    }
}