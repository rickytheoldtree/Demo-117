using System.Collections;
using Demo_117.Services;
using RicKit.Particle;
using RicKit.RFramework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demo_117.GamePlay
{
    public class CannonBall : MonoBehaviour, ICanGetLocator<Entity>
    {
        public Vector3 StartPosition { get; private set; }
        private Rigidbody rb;
        private TrailRenderer trailRenderer;
        private MeshRenderer meshRenderer;
        private IObjectPoolService objectPoolService;
        private IParticleService particleService;
        private void Awake()
        {
            // 获取对象池服务
            this.TryGetService(out objectPoolService);
            this.TryGetService(out particleService);
            // 获取组件
            rb = GetComponent<Rigidbody>();
            trailRenderer = GetComponent<TrailRenderer>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        // 回收时重置状态
        public void OnRelease()
        {
            StopAllCoroutines();
            rb.isKinematic = true;
            trailRenderer.Clear();
            gameObject.SetActive(false);
        }

        public void Shoot(Transform launchPoint, float power)
        {
            meshRenderer.enabled = true;
            StartPosition = launchPoint.position;
            
            // 设置初始位置和旋转
            transform.SetPositionAndRotation(launchPoint.position, launchPoint.rotation);

            // 偏移角度（度）
            float maxDeviation = 1f; // 可调，例如 1 度
            // 随机生成旋转：先绕右轴偏移，再绕前轴随机旋转一圈
            Quaternion randomRotation =
                Quaternion.AngleAxis(Random.Range(0f, 360f), launchPoint.forward) *
                Quaternion.AngleAxis(Random.Range(0f, maxDeviation), launchPoint.up);

            // 计算方向
            Vector3 direction = (randomRotation * launchPoint.forward).normalized;

            // 设置速度
            rb.isKinematic = false; // 确保物理模拟开启
            rb.velocity = direction * power;
            
            // 等待15秒后自动释放 (如果在此之前没有碰撞到地面)
            StartCoroutine(DelayRelease(15f));
        }

        private IEnumerator DelayRelease(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            objectPoolService.CannonBallPool.Release(this);
        }

        public void OnHitGround()
        {
            // 播放音效（可选）
            // audioService.PlaySound("HitGround");
            
            // 这里可以添加其他逻辑，例如生成粒子效果等
            particleService.QueueParticle("Explosion", transform.position);
            
            meshRenderer.enabled = false; // 隐藏炮弹模型，显示轨迹
            rb.isKinematic = true; // 停止物理模拟，保持在地面上
            StartCoroutine(DelayRelease(5f)); // 3秒后回收，给轨迹留点时间
        }
    }
}