using RicKit.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace Demo_117.UI.UIPanels
{
    public class UIQuitConfirm : PopUIPanel
    {
        [SerializeField] private Button btnConfirm;

        protected override void Awake()
        {
            base.Awake();
            btnConfirm.onClick.AddListener(OnBtnQuitClick);
        }

        private void OnBtnQuitClick()
        {
            if (Application.isEditor)
            {
                // 在编辑器中，停止播放模式
                UnityEditor.EditorApplication.isPlaying = false;
                return;
            }

            // 在构建的应用程序中，退出应用程序
            Application.Quit();
        }
    }
}