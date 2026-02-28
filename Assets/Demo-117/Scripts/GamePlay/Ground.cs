using Demo_117.GamePlay.Cmds;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.GamePlay
{
    public class Ground : MonoBehaviour, ICanGetLocator<Entity>
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out CannonBall cannonBall)) return;
            this.SendCommand<CmdOnHitGround, CmdOnHitGround.Args>(new CmdOnHitGround.Args { cannonBall = cannonBall });
        }
    }
}