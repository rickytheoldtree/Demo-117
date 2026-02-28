using RicKit.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace Demo_117.UI.UIPanels
{
    public class UIPause : PopUIPanel
    {
        [SerializeField] private Button btnProfile, btnQuit;
        protected override void Awake()
        {
            base.Awake();
            btnProfile.onClick.AddListener(OnBtnProfileClick);
            btnQuit.onClick.AddListener(OnBtnQuitClick);
        }

        private void OnBtnProfileClick()
        {
            // 隐藏后显示UIProfileEdit面板
            UI.HideThenShowUI<UIProfileEdit>();
            // 或者直接显示UIProfileEdit面板
            // UI.ShowUI<UIProfileEdit>();
        }

        private void OnBtnQuitClick()
        {
            // 显示UIQuitConfirm面板
            UI.ShowUI<UIQuitConfirm>();
        }
    }
}