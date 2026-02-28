using Demo_117.Services;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.GamePlay.Cmds
{
    public class CmdOnHitGround : AbstractCommandOnlyArgs<CmdOnHitGround.Args>
    {
        private IInGameDataService inGameDataService;
        
        // 注意！！！！ Init只会在第一次执行时调用一次
        // 但所有的Cmd都会缓存，所以请在Init中获取需要的服务
        public override void Init()
        {
            // 获取需要的服务
            this.TryGetService(out inGameDataService);
        }

        public struct Args
        {
            public CannonBall cannonBall;
        }

        public override void Execute(Args args)
        {
            var distance = Vector3.Distance(args.cannonBall.transform.position, args.cannonBall.StartPosition);
            //根据power值来决定获得分数
            int score = Mathf.FloorToInt(distance * 10);
            
            //增加分数，由于inGameDataService.Score受到修改，所有注册的事件都会触发，包括持久化数据，以及更新UIGame上面的txtScore文本
            inGameDataService.Score.Value += score;
            
            // 处理完数据层，回调表现层，表现层可以根据这个事件来播放爆炸特效、音效等
            args.cannonBall.OnHitGround();
        }
    }
}