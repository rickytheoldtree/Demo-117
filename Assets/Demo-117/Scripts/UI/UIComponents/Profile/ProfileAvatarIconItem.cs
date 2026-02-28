using Demo_117.Services;
using RicKit.RFramework;
using RicKit.RFramework.UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Demo_117.UI.UIComponents.Profile
{
    public class ProfileAvatarIconItem : BindablePropertyButton<int>, ICanGetLocator<Entity>
    {
        [SerializeField] private Image imgContent;
        [SerializeField] private GameObject goSelected;

        protected override void InitUI(bool selected)
        {
            // 首次初始化：显示选中态并加载对应资源。
            goSelected.SetActive(selected);
            imgContent.sprite = this.GetService<IProfileService>().GetAvatarIcon(id);
        }

        protected override void UpdateUI(bool selected)
        {
            // 仅更新选中态，避免重复加载图片。
            goSelected.SetActive(selected);
        }
    }
}