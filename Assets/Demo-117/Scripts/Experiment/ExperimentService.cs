using Demo_117.Experiment.Data;
using Demo_117.Services;
using RicKit.Experiment;
using RicKit.RFramework;

namespace Demo_117.Experiment
{
    public interface IExperimentService : IExperimentManager, IService
    {
    }
    public class ExperimentService : BaseExperimentManager, IExperimentService
    {
        public bool IsInitialized { get; set; }
        private IServiceLocator locator;
        private IPrefsService prefsService;
        public void Init()
        {
            this.TryGetService(out prefsService);
            InitExperimentManager();
            RegisterExperiment<ExpDifficulty10100>();
        }
        protected override string GetString(string key)
        {
            return prefsService.GetString(key);
        }

        protected override void Save()
        {
            prefsService.Save();
        }

        protected override void SetString(string key, string value)
        {
            prefsService.SetString(key, value);
        }

        protected override int GetInt(string key)
        {
            return prefsService.GetInt(key);
        }

        protected override void SetInt(string key, int value)
        {
            prefsService.SetInt(key, value);
        }
 
        public void DeInit()
        {
            
        }

        public void SetLocator(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public IServiceLocator GetLocator()
        {
            return locator;
        }

        public void Start()
        {
        }
    }
}