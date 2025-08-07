using RicKit.RFramework;

namespace Demo_117.Services
{
    public interface IInGameDataService : IService
    {
        BindableProperty<int> Score { get; }
    }
    
    public class InGameDataService : AbstractService, IInGameDataService
    {
        private const string ScoreKey = "InGameScore";
        public BindableProperty<int> Score { get; } = new BindableProperty<int>(0);

        public override void Init()
        {
            // 初始化游戏数据
            
            // 获取IPrefService服务
            this.TryGetService(out IPrefService prefService);
            
            // 从PlayerPrefs中获取初始分数，然后注册一个监听器当分数变化时更新PlayerPrefs
            Score.SetWithoutInvoke(prefService.GetInt(ScoreKey, 0));
            Score.RegisterAndInvoke(newValue => prefService.SetInt(ScoreKey, newValue));
        }
    }
}