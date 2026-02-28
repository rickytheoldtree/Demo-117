using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.GamePlay.Cmds
{
    public struct ShootEvent
    {
        public float power;
    }
    public class CmdShoot : AbstractCommandOnlyArgs<CmdShoot.Args>
    {
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
            
            //处理完逻辑层，开始广播给表现层
            this.SendEvent(new ShootEvent
            {
                power = args.power,
            });
        }
    }
}