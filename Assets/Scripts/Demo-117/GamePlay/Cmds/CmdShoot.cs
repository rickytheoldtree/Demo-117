using Demo_117.Services;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.GamePlay.Cmds
{
    public struct ShootEvent
    {
        public float power;
        public int scoreAdded;
    }
    public class CmdShoot : AbstractCommandOnlyArgs<CmdShoot.Args>
    {
        //private IAudioService audioService;
        private IInGameDataService inGameDataService;
        
        // 注意！！！！ Init只会在第一次执行时调用一次
        // 但所有的Cmd都会缓存，所以请在Init中获取需要的服务
        public override void Init()
        {
            // 获取需要的服务
            // this.TryGetService(out audioService);
            this.TryGetService(out inGameDataService);
        }

        public struct Args
        {
            public float power;
        }

        public override void Execute(Args args)
        {
            // 这里可以添加射击逻辑
            // 例如，创建子弹、播放射击音效等
            // args可以包含射击的方向、速度等信息
            
            // 示例：打印射击角度
            Debug.Log($"Shooting with power: {args.power}");
            
            //根据power值来决定获得分数
            int score = Mathf.FloorToInt(args.power * 10);
            
            //增加分数，由于inGameDataService.Score受到修改，所有注册的事件都会触发，包括持久化数据，以及更新UIGame上面的txtScore文本
            inGameDataService.Score.Value += score;
            
            //处理完逻辑层，开始广播给表现层
            this.SendEvent(new ShootEvent
            {
                power = args.power,
                scoreAdded = score 
            });
        }
    }
}