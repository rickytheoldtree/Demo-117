using Demo_117.Services;
using RicKit.RFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Demo_117.UI.UIComponents.Profile
{
    public class AvatarBase : MonoBehaviour, ICanGetLocator<Entity>
    {
        [SerializeField] private Image imgIcon, imgFrame;

        public void SetIcon(int avatar)
        {
            // 通过 ProfileService 获取头像资源。
            imgIcon.sprite = this.GetService<IProfileService>().GetAvatarIcon(avatar);
        }
        
        public void SetFrame(int frame)
        {
            // 通过 ProfileService 获取头像框资源。
            imgFrame.sprite = this.GetService<IProfileService>().GetAvatarFrame(frame);
        }

        public void Set(int id, int frameId)
        {
            // 同时设置头像与头像框。
            SetIcon(id);
            SetFrame(frameId);
        }
    }
}