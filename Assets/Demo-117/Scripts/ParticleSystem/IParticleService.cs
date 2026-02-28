using Demo_117.Services;
using RicKit.RFramework;

namespace RicKit.Particle
{
    public interface IParticleService : IParticleSystemSystem, IService
    {
    }

    public class ParticleService : AbstractParticleSystemSystem, IParticleService
    {
        public bool IsInitialized { get; set; }
        private IServiceLocator locator;
        private IAssetService assetService;

        public new void Init()
        {
            this.TryGetService(out assetService);
            base.Init();
        }

        public void DeInit()
        {
        }

        IServiceLocator ICanGetLocator.GetLocator() => locator;

        void ICanSetLocator.SetLocator(IServiceLocator locator)
        {
            this.locator = locator;
        }

        protected override T Load<T>(string path)
        {
            return assetService.Load<T>(path);
        }

        protected override string PathFormat => "Assets/Demo-117/Effects/Prefabs/{0}.prefab";

        public void Start()
        {
        }
    }
}