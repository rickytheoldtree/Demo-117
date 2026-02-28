using Cysharp.Threading.Tasks;
using Demo_117.Services;
using Demo_117.UI.UIPanels;
using DG.Tweening;
using RicKit.RFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo_117
{
    //游戏的启动类
    public class Splash : MonoBehaviour, ICanGetLocator<Entity>
    {
        private IUIService uiService;
        private ISceneService sceneService;
        private IAssetService assetService;
        [SerializeField]
        private CanvasGroup cgLoading; //简陋的Splash界面

        [SerializeField] private RectTransform rtLoadingRoot;
        private void Awake()
        {
            Application.targetFrameRate = 120;
            //初始化框架
            Entity.Initialize();
            
            //获取UI服务，请注意，框架中获取服务的方式是通过接口获取的，这样可以更好地解耦和测试
            this.TryGetService(out uiService);
            this.TryGetService(out sceneService);
            this.TryGetService(out assetService);
        }

        private void Start()
        {
            StartAsync().Forget();
            return;
            async UniTask StartAsync()
            {
                //显示Loading界面
                await ShowLoading();

                //加载游戏场景
                await sceneService.LoadSceneAsync("Game", LoadSceneMode.Additive);

                //显示主界面，具体用法请参考文档
                //https://github.com/rickytheoldtree/com.rickit.ui/blob/main/README.zh-CN.md
                await uiService.ShowUIAsync<UIGame>(p => p.Init(new UIGame.Args
                {
                    someParameter = 666
                }));

                //隐藏Loading界面
                await cgLoading.DOFade(0, 0.5f).AsyncWaitForCompletion();

                //销毁Splash场景
                sceneService.UnloadScene("Splash");
            }
        }

        private async UniTask ShowLoading()
        {
            //这里我们直接在Splash界面上显示一个文本，实际项目中可以根据需要设计更复杂的界面，并且可以在加载过程中根据实际进度更新界面
            //通过加载的方式而不选择直接放在splash场景的理由主要是为了防止字体资源打包冗余
            var prefab = await assetService.LoadAssetAsync<GameObject>("Assets/Demo-117/Prefabs/Splash/txtLoading.prefab");
            var txt = Instantiate(prefab, rtLoadingRoot).GetComponent<TMPro.TMP_Text>();
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
            await txt.DOFade(1, 0.5f).AsyncWaitForCompletion();
        }
    }
}

