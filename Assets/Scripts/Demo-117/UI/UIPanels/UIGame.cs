using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Demo_117.GamePlay.Cmds;
using Demo_117.Services;
using RicKit.RFramework;
using RicKit.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Demo_117.UI.UIPanels
{
    public class UIGame : FadeUIPanel, ICanGetLocator<Entity>
    {
        [SerializeField]
        private Button btnShoot, btnPause;
        [SerializeField]
        private TMP_Text txtScore;
        private int someParameter;
        private IInGameDataService inGameDataService;
        protected override void Awake()
        {
            base.Awake();
            // 获取服务
            this.TryGetService(out inGameDataService);
            
            // 绑定按钮点击事件
            btnShoot.onClick.AddListener(OnShootClick);
            btnPause.onClick.AddListener(OnPauseClick);
            
            // 注册分数更新事件
            inGameDataService.Score.RegisterAndInvoke(OnScoreUpdated);
        }

        private void OnDestroy()
        {
            // 解绑按钮点击事件，不过Unity按钮在销毁时会自动解绑，所以这一步可以省略
            btnShoot.onClick.RemoveListener(OnShootClick);
            
            // 注销分数更新事件
            inGameDataService.Score.UnRegister(OnScoreUpdated);
        }

        private void OnShootClick()
        {
            // 发送射击命令，模拟射击行为
            // CmdShoot.Args 是一个包含射击参数的结构体，这里我们随机生成
            // power 的值，范围从 10 到 50
            this.SendCommand<CmdShoot, CmdShoot.Args>(new CmdShoot.Args
            {
                power = Random.Range(10, 51)
            });
        }
        
        private void OnPauseClick()
        {
            // 发送暂停命令，模拟游戏暂停行为
            //如果不在UI系统的界面里调用，则请走IUIService
            // this.GetService<IUIService>().ShowUI<UIPause>();
            UI.ShowUI<UIPause>();
        }
        
        // 分数更新回调
        // 这个方法会在分数更新时被调用，更新 UI 上的分数文本
        private void OnScoreUpdated(int score)
        {
            txtScore.text = $"Score: {score}";
        }

        public void Init(Args args)
        {
            // 这里可以添加一些初始化逻辑，比如设置面板的标题、加载数据等
            // args 可以包含一些初始化参数
            someParameter = args.someParameter;
        }

        //可以重写界面的出现动画
        protected override UniTask OnAnimationIn(CancellationToken cancellationToken)
        {
            return base.OnAnimationIn(cancellationToken);
        }

        //可以重写界面的消失动画
        protected override UniTask OnAnimationOut(CancellationToken cancellationToken)
        {
            return base.OnAnimationOut(cancellationToken);
        }
        
        public struct Args
        {
            public int someParameter; // 可以根据需要添加更多参数
        }
        
        // 处理ESC键点击事件
        // 这里可以添加一些逻辑，比如关闭面板或返回主菜单等
        // 注意：如果这个面板不需要处理ESC键，可以不实现这个方法
        public override void OnESCClick()
        {
            //UI.Back();
        }
    }
}