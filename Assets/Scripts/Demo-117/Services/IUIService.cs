using Cysharp.Threading.Tasks;
using RicKit.RFramework;
using RicKit.UI;

namespace Demo_117.Services
{
    public interface IUIService : IUIManager, IService
    {
        UniTask LockFor(int milliseconds);
    }

    public class UIService : UIManager, IUIService
    {
        private IServiceLocator serviceLocator;
        IServiceLocator ICanGetLocator.GetLocator() => serviceLocator;
        void ICanSetLocator.SetLocator(IServiceLocator locator) => serviceLocator = locator;
        
        public async UniTask LockFor(int milliseconds)
        {
            //锁定输入，防止在某些操作中用户进行其他操作
            SetLockInput(true);
            //等待指定的毫秒数
            await UniTask.Delay(milliseconds);
            //解锁输入
            SetLockInput(false);
            
            //PS. SetLockInput(true/false); 使用的是计数的方式来判断是否遮挡用户的UI输入，当计数为0时才会解锁输入
        }

        public bool IsInitialized { get; set; }
        public void Init() => Initiate(this.GetService<IAssetService>());

        public void DeInit()
        {
            
        }

        public void Start()
        {
            
        }
    }

    //可以自行添加UI相关的扩展方法
    public static class UIExtension
    {
        //例如：显示加载界面
        /*public static void Loading(this IUIManager ui, Func<UniTask> task, bool showBanner)
        {
            ui.ShowUIUnmanagableAsync<UILoading>(panel =>
            {
                panel.SetTask(task, showBanner);
            }).ContinueWith(() =>
            {
                var loadingPanel = ui.GetUI<UILoading>();
                if (loadingPanel && loadingPanel.IsShow)
                {
                    loadingPanel.OnHideAsync().Forget();
                }
            }).Forget();
        }*/
    }
}