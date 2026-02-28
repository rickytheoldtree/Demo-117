using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RicKit.Particle
{
    public interface IParticleSystemSystem
    {
        /// <summary>
        /// 加载粒子系统，不缓存，请手动销毁
        /// </summary>
        ParticleSystem CreateParticle(string name);
        /// <summary>
        /// 将粒子播放请求加入队列，只支持第一帧就全部渲染的粒子
        /// </summary>
        void QueueParticle(string name, Vector3 pos, float scale = 1, Action<ParticleSystem> beforePlay = null);
        /// <summary>
        /// 播放粒子，不缓存，播放完毕后可选择销毁
        /// </summary>
        ParticleSystem PlayParticle(string name, Vector3 pos, bool destroy = true);

        GameObject CreateGameObject(string name);
    }
    public abstract class AbstractParticleSystemSystem : IParticleSystemSystem
    {
        protected abstract T Load<T>(string path) where T : Object;
        protected abstract string PathFormat { get; }

        private readonly Dictionary<string, ParticleSystem> mParticlesCache =
            new Dictionary<string, ParticleSystem>();

        private readonly Dictionary<string, ParticleSystem> mPrefabCache =
            new Dictionary<string, ParticleSystem>();
        private ParticlesCtrl particlesCtrl;

        protected void Init()
        {
            var go = new GameObject("Particles", typeof(ParticlesCtrl));
            Object.DontDestroyOnLoad(go);
            particlesCtrl = go.GetComponent<ParticlesCtrl>();
            particlesCtrl.updateAction = Update;
        }

        public ParticleSystem CreateParticle(string name)
        {
            return Object.Instantiate(GetPrefab(name), particlesCtrl.transform);
        }
        private ParticleSystem GetPrefab(string name)
        {
            if (mPrefabCache.TryGetValue(name, out var p)) return p;
            var prefab = Load<GameObject>(string.Format(PathFormat, name)).GetComponent<ParticleSystem>();
            mPrefabCache.Add(name, prefab);
            return mPrefabCache[name];
        }
        private ParticleSystem GetParticleSystem(string name)
        {
            if (mParticlesCache.TryGetValue(name, out var ps)) return ps;
            ps = Object.Instantiate(GetPrefab(name), particlesCtrl.transform);
            ps.Set(ParticleSystemSimulationSpace.World, ParticleSystemStopAction.Disable);
            mParticlesCache.Add(name, ps);
            return ps;
        }

        public void QueueParticle(string name, Vector3 pos, float scale = 1, Action<ParticleSystem> beforePlay = null)
        {
            if (!mParticleQueue.ContainsKey(name))
            {
                mParticleQueue.Add(name, new Queue<ParticleInfo>());
            }

            mParticleQueue[name].Enqueue(new ParticleInfo
            {
                pos = pos,
                scale = scale,
                beforePlay = beforePlay
            });
        }

        public ParticleSystem PlayParticle(string name, Vector3 pos, bool destroy = true)
        {
            var ps = Object.Instantiate(GetPrefab(name), particlesCtrl.transform);
            ps.transform.position = pos;
            if (!destroy) 
                return ps;
            ps.Set(ParticleSystemStopAction.Destroy);
            return ps;
        }

        public GameObject CreateGameObject(string name)
        {
            var prefab = Load<GameObject>(PathFormat + name);
            if (!prefab) return null;
            var go = Object.Instantiate(prefab, particlesCtrl.transform);
            go.SetActive(true);
            return go;
        }

        private void Play(string name, ParticleInfo info)
        {
            var ps = GetParticleSystem(name);
            ps.gameObject.SetActive(true);
            ps.transform.localScale = Vector3.one * info.scale;
            ps.transform.position = info.pos;
            info.beforePlay?.Invoke(ps);
            ps.Play();
        }
        private struct ParticleInfo
        {
            public Vector3 pos;
            public float scale;
            public Action<ParticleSystem> beforePlay;
        }
        private readonly Dictionary<string, Queue<ParticleInfo>> mParticleQueue = new Dictionary<string, Queue<ParticleInfo>>();

        private void Update()
        {
            foreach (var kv in mParticleQueue.Where(kv => kv.Value.Count > 0))
            {
                Play(kv.Key, kv.Value.Dequeue());
            }
        }
    }

    public class ParticlesCtrl : MonoBehaviour
    {
        public Action updateAction;

        public void Update()
        {
            updateAction?.Invoke();
        }
    }

    public static class ParticleSystemExtension
    {
        public static void Set(this ParticleSystem ps, ParticleSystemSimulationSpace space,
            ParticleSystemStopAction stopAction)
        {
            var all = ps.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in all)
            {
                var main = p.main;
                main.simulationSpace = space;
                if(p == ps) main.stopAction = stopAction;
            }
        }
        
        public static void Set(this ParticleSystem ps, ParticleSystemSimulationSpace space)
        {
            var all = ps.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in all)
            {
                var main = p.main;
                main.simulationSpace = space;
            }
        }
        
        public static void Set(this ParticleSystem ps, ParticleSystemStopAction stopAction)
        {
            var main = ps.main;
            main.stopAction = stopAction;
        }
        
        public static void DelayDestroy(this ParticleSystem ps, float f)
        {
            if(!ps) return;
            ps.gameObject.DelayDestroy(f);
        }
        public static void DelayDestroy(this GameObject go, float f)
        {
            if(!go) return;
            go.AddComponent<DelayDestroyer>().DelayDestroy(f);
        }
    }
    
    public class DelayDestroyer : MonoBehaviour
    {
        public void DelayDestroy(float f)
        {
            StartCoroutine(DelayDestroyCoroutine(f));
        }
        private IEnumerator DelayDestroyCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}