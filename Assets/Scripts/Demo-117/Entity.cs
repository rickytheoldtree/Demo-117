using Demo_117.Services;
using RicKit.RFramework;

namespace Demo_117
{
    // 这是框架主题，为单例；完整文档见：https://github.com/rickytheoldtree/com.rickit.rframework/blob/main/README.zh-CN.md
    public class Entity : ServiceLocator<Entity>
    {
        // 这里可以添加一些全局的服务或数据
        //注册服务时，泛型最好使用接口类型，这样可以更好地解耦和测试，在后续获取服务时，用接口获取
        public override void Init()
        {
            #region 工具类（首先初始化工具类服务，一般这些服务不会依赖其他服务）
            
            //存储服务
            RegisterService<IPrefService>(new PlayerPrefService());
            
            //资源加载相关
            //RegisterService<IAssetService>(new AddressablesAssetService());
            RegisterService<IAssetService>(new ResourcesAssetService());
            
            //场景加载服务
            RegisterService<ISceneService>(new SceneService());
            
            //UI服务
            RegisterService<IUIService>(new UIService());
            
            //Tables服务
            RegisterService<ITablesService>(new TablesService());
            
            //自动保存服务
            RegisterService<IAutoSaveService>(new AutoSaveService());
            
            #endregion

            // 然后优先注册所有持久化数据的服务
            #region 数据服务
            
            //局内数据服务
            RegisterService<IInGameDataService>(new InGameDataService());
            
            //游戏局内存档数据服务
            //RegisterService<IGameSaveService>(new GameSaveService());

            //游戏设置数据服务
            //RegisterService<ISettingsDataService>(new SettingsDataService());
            #endregion
            
            
            //业务服务可以在这里注册
            #region 业务服务
            
            //游戏服务（玩法逻辑）一般来说会设计的很大，不过可以通过拆分成小的模块来管理，也可以善用Command来减负
            //RegisterService<IGameService>(new GameService());
            
            //本地化服务（处理多语言）
            //RegisterService<ILocalizationService>(new LocalizationService());
            
            //试验服务（用于实验性功能或AB测试）
            //RegisterService<IExperimentService>(new ExperimentService());
            
            //广告服务（用于处理广告相关逻辑）
            //RegisterService<IADService>(new ADService());
            
            //支付服务（用于处理应用内购买）
            //RegisterService<IIAPService>(new IAPService());
            
            //红点系统 （用于处理游戏中的通知和提示）
            //RegisterService<IRedDotService>(new RedDotService());
            #endregion
        }
    }
}

