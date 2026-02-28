using Demo_117.GamePlay;
using RicKit.RFramework;
using UnityEngine;
using UnityEngine.Pool;

namespace Demo_117.Services
{
    public interface IObjectPoolService : IService
    {
        public IObjectPool<CannonBall> CannonBallPool { get; }
    }
    
    public class ObjectPoolService : AbstractService, IObjectPoolService
    {
        private IAssetService assetService;
        private CannonBall cannonBallPrefab;
        private ObjectPool<CannonBall> cannonBallPool;
        public override void Init()
        {
            base.Init();
            //依赖注入，获取需要的服务
            this.TryGetService(out assetService);
        }

        public IObjectPool<CannonBall> CannonBallPool
        {
            get
            {
                if (cannonBallPool != null) return cannonBallPool;
                
                //加载预制体（展示的是Addressables的索引）
                cannonBallPrefab = assetService.Load<GameObject>("Assets/Demo-117/Prefabs/CannonBall.prefab").GetComponent<CannonBall>();
                //cannonBallPrefab = assetService.Load<GameObject>("Prefabs/CannonBall"); //如果使用Resources加载的话路径是类似这样的
                
                cannonBallPool = new ObjectPool<CannonBall>(
                    () => Object.Instantiate(cannonBallPrefab),
                    cannonBall => cannonBall.gameObject.SetActive(true),
                    cannonBall => cannonBall.OnRelease(),
                    cannonBall => Object.Destroy(cannonBall.gameObject),
                    false, 10, 20);
                return cannonBallPool;
            }
        }
    }
}