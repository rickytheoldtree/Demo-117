using Cysharp.Threading.Tasks;
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
                obj =>
                {
                    //清理炮弹状态
                    if (obj.TryGetComponent(out Rigidbody rb))
                        rb.velocity = Vector3.zero;
                    if (obj.TryGetComponent<TrailRenderer>(out var trailRenderer))
                        trailRenderer.Clear();
                    obj.SetActive(false);
                },
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
            // 从对象池获取并初始化炮弹
            var cannonBall = cannonBallPool.Get();
            cannonBall.transform.SetPositionAndRotation(launchPoint.position, launchPoint.rotation);
            cannonBall.SetActive(true);

            // 偏移角度（度）
            float maxDeviation = 1f; // 可调，例如 1 度
            // 随机生成旋转：先绕右轴偏移，再绕前轴随机旋转一圈
            Quaternion randomRotation =
                Quaternion.AngleAxis(Random.Range(0f, 360f), launchPoint.forward) *
                Quaternion.AngleAxis(Random.Range(0f, maxDeviation), launchPoint.up);

            // 计算方向
            Vector3 direction = (randomRotation * launchPoint.forward).normalized;

            // 设置速度
            if (cannonBall.TryGetComponent(out Rigidbody rb))
                rb.velocity = direction * e.power;

            // 延迟释放炮弹
            UniTask.Delay(15000).ContinueWith(() =>
            {
                if(cannonBall) cannonBallPool.Release(cannonBall);
            });
            
            //可以在这里添加更多逻辑，比如播放发射音效等
        }
    }
}