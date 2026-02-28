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
        
        private IObjectPoolService objectPoolService;
        public void Awake()
        {
            //获取对象池服务
            this.TryGetService(out objectPoolService);
            //注册发射事件
            this.RegisterEvent<ShootEvent>(OnShoot);
            //注册转动炮台事件
            this.RegisterEvent<RotateCannonEvent>(RotateCannon);
        }

        private void OnDestroy()
        {
            //取消注册事件
            this.UnRegisterEvent<ShootEvent>(OnShoot);
            this.UnRegisterEvent<RotateCannonEvent>(RotateCannon);
        }
        
        private void OnShoot(ShootEvent e)
        {
            //从对象池中获取一个炮弹
            // 从对象池获取并初始化炮弹
            var cannonBall = objectPoolService.CannonBallPool.Get();
            cannonBall.Shoot(launchPoint, e.power);
        }
        
        private void RotateCannon(RotateCannonEvent e)
        {
            float rotationSpeed = 0.1f; // 旋转速度

            // 当前欧拉角
            Vector3 euler = transform.localEulerAngles;

            // 转成 -180 ~ 180
            if (euler.x > 180f) euler.x -= 360f;
            if (euler.y > 180f) euler.y -= 360f;

            // 增加旋转量
            euler.x = e.reverseY ? euler.x - e.delta.y * rotationSpeed : euler.x + e.delta.y * rotationSpeed; // 注意鼠标Y要反向
            euler.y += e.delta.x * rotationSpeed;

            // 限制范围
            euler.x = Mathf.Clamp(euler.x, 15f, 60f); // X轴范围
            euler.y = Mathf.Clamp(euler.y, -45f, 45f); // Y轴范围

            // 应用回去
            transform.localEulerAngles = euler;
        }
    }
}