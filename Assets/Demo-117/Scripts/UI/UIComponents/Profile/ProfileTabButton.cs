using RicKit.RFramework;
using RicKit.RFramework.UIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Demo_117.UI.UIComponents.Profile
{
    public class ProfileTabButton : BindablePropertyButton<int>, ICanGetLocator<Entity>
    {
        [SerializeField] private Image img;
        [SerializeField] private Sprite spriteSelected, spriteUnselected;
        [SerializeField] private GameObject content;

        protected override void InitUI(bool selected)
        {
            // 首次初始化：根据选中态切换内容与按钮外观。
            content.SetActive(selected);
            img.sprite = selected ? spriteSelected : spriteUnselected;
        }

        protected override void UpdateUI(bool selected)
        {
            if (lastSelected == selected) return;
            // 选中状态变化时同步 UI。
            content.SetActive(selected);
            img.sprite = selected ? spriteSelected : spriteUnselected;
        }
    }
}