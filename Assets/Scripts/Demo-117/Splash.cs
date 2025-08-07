using System;
using Cysharp.Threading.Tasks;
using Demo_117.Services;
using Demo_117.UI.UIPanels;
using DG.Tweening;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117
{
    //游戏的启动类
    public class Splash : MonoBehaviour, ICanGetLocator<Entity>
    {
        private IUIService uiService;
        private ISceneService sceneService;
        [SerializeField]
        private CanvasGroup cgLoading; //简陋的Splash界面
        private void Awake()
        {
            //初始化框架
            Entity.Initialize();
            
            //获取UI服务，请注意，框架中获取服务的方式是通过接口获取的，这样可以更好地解耦和测试
            this.TryGetService(out uiService);
            this.TryGetService(out sceneService);
        }

        private async void Start()
        {
            //加载游戏场景
            await sceneService.LoadSceneAsync("Game");
            
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
}

