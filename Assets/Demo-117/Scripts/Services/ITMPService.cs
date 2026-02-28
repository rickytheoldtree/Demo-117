using RicKit.RFramework;
using TMPro;

namespace Demo_117.Services
{
    public interface ITMPService : IService
    {
    }
    public class TMPService : AbstractService, ITMPService
    {
        private IAssetService assetService;
        public override void Init()
        {
            this.TryGetService(out assetService);
            var defaultSettings = assetService.Load<TMP_Settings>("Assets/TextMesh Pro/TMP Settings.asset");
            //用反射设置默认设置s_Instance
            var type = typeof(TMP_Settings);
            var field = type.GetField("s_Instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            field?.SetValue(null, defaultSettings);
        }
    }
}