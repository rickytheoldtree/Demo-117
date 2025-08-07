using Demo_117.Services;
using RicKit.RFramework;

namespace Demo_117
{
    // 这是框架主题，为单例
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
            
            #endregion

            #region 数据服务

            //局内数据服务
            RegisterService<IInGameDataService>(new InGameDataService());

            #endregion
        }
    }
}

