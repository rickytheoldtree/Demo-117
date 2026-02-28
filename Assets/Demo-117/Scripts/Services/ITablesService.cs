using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.Services
{
    // 读取配置表的服务接口
    public interface ITablesService : IService
    {
        //该表为策划配好后，由luban插件生成的配置表，具体用法查看https://luban.doc.code-philosophy.com/docs/intro
        public cfg.Tables Tables { get; }
    }

    //配置表读取服务实现
    public class TablesService : AbstractService, ITablesService
    {
        private IAssetService assetService;

        public override void Init()
        {
            this.TryGetService(out assetService);
            Tables = new cfg.Tables(path =>
            {
                // 这里的path是配置表生成的代码里传入的参数，这里是使用Addressables加载的
                var ta = assetService.Load<TextAsset>($"Assets/Config/Json/{path}.json");
                // var ta = assetService.Load<TextAsset>($"Config/Json/{path}"); //如果使用Resources加载的话路径是类似这样的
                return JsonConvert.DeserializeObject<JArray>(ta.text);
            });
        }

        public cfg.Tables Tables { get; private set; }
    }
}