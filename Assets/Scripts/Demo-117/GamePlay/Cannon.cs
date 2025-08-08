using Demo_117.GamePlay.Cmds;
using Demo_117.Services;
using RicKit.RFramework;
using UnityEngine;
using UnityEngine.Pool;

namespace Demo_117.GamePlay
{
    public class Cannon : MonoBehaviour, ICanGetLocator<Entity>
    {
        [SerializeField] private Transform launchPoint; //发射点
        private GameObject cannonBallPrefab; //这里可以使用SerializeField来在Inspector中设置预制体，这里演示使用代码加载
        private ObjectPool<GameObject> cannonBallPool; //对象池，用于管理炮弹实例
        private IAssetService assetService;
        
        public void Awake()
        {
            //依赖注入，获取需要的服务
            this.TryGetService(out assetService);
            //加载预制体
            cannonBallPrefab = assetService.Load<GameObject>("Prefabs/CannonBall");
            //初始化对象池
            cannonBallPool = new ObjectPool<GameObject>(
                () => Instantiate(cannonBallPrefab, transform.position, Quaternion.identity),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                Destroy,
                false, 10, 20);
            
            //注册发射事件
            this.RegisterEvent<ShootEvent>(OnShoot);
        }

        private void OnDestroy()
        {
            //清理对象池
            cannonBallPool.Clear();
            cannonBallPool.Dispose();
            
            //取消注册事件
            this.UnRegisterEvent<ShootEvent>(OnShoot);
        }
        
        private void OnShoot(ShootEvent e)
        {
            //从对象池中获取一个炮弹
            var cannonBall = cannonBallPool.Get();
            cannonBall.transform.position = launchPoint.position;
            cannonBall.transform.rotation = launchPoint.rotation;
            cannonBall.SetActive(true);
            
            //设置炮弹的速度和方向
            var rb = cannonBall.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = launchPoint.forward * e.power;
            }
            
            //可以在这里添加更多逻辑，比如播放发射音效等
        }
    }
}