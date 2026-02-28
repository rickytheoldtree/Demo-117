using System.Collections.Generic;
using System.Linq;
using Demo_117.SO;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.Services
{
    public interface IProfileService : IService
    {
        BindableProperty<string> PlayerName { get; }
        BindableProperty<string> PlayerNameTemp { get; }
        BindableProperty<int> PlayerAvatarIcon { get; }
        BindableProperty<int> PlayerAvatarIconTemp { get; }
        BindableProperty<int> PlayerAvatarFrame { get; }
        BindableProperty<int> PlayerAvatarFrameTemp { get; }
        BindableProperty<bool> ProfileEntered { get; }
        Sprite GetAvatarIcon(int id);
        Sprite GetAvatarFrame(int id);
        IEnumerable<int> GetAvatarIcons();
        IEnumerable<int> GetAvatarFrames();
        int GetRandomAvatarIcon();
        int GetRandomAvatarFrame();
    }

    public class ProfileService : AbstractService, IProfileService
    {
        // PlayerPrefs 存储键。
        private const string PlayerNameKey = "PlayerName";
        private const string PlayerAvatarKey = "PlayerAvatarIcon";
        private const string ProfileEnteredKey = "ProfileChanged";
        private const string PlayerFrameKey = "PlayerAvatarFrame";

        public BindableProperty<string> PlayerName { get; } = new();
        public BindableProperty<string> PlayerNameTemp { get; } = new();
        public BindableProperty<int> PlayerAvatarIcon { get; } = new();
        public BindableProperty<int> PlayerAvatarIconTemp { get; } = new();
        public BindableProperty<int> PlayerAvatarFrame { get; } = new();
        public BindableProperty<int> PlayerAvatarFrameTemp { get; } = new();
        public BindableProperty<bool> ProfileEntered { get; } = new();
        private List<Sprite> AvatarIconList { get; } = new();
        private List<Sprite> AvatarFrameList { get; } = new();

        public override void Init()
        {
            var assetService = this.GetService<IAssetService>();
            // 资源加载：头像与头像框统一从 ScriptableObject 列表中读取。
            var avatars = assetService.Load<SpriteList>("Assets/Demo-117/Sprites/Icons.asset");
            foreach (var sprite in avatars.sprites)
            {
                AvatarIconList.Add(sprite);
            }
            
            // 资源加载：头像框列表。
            var frames = assetService.Load<SpriteList>("Assets/Demo-117/Sprites/Frames.asset");
            foreach (var sprite in frames.sprites)
            {
                AvatarFrameList.Add(sprite);
            }

            var prefService = this.GetService<IPrefsService>();
            
            // 用户名：没有存档时创建默认名，并持久化。
            if (prefService.HasKey(PlayerNameKey))
                PlayerName.SetWithoutInvoke(prefService.GetString(PlayerNameKey, null));
            else
            {
                var name = $"Player_0";
                PlayerName.SetWithoutInvoke(name);
                prefService.SetString(PlayerNameKey, name);
            }
            PlayerName.Register(i => prefService.SetString(PlayerNameKey, i));
            
            // 头像：绑定存档写回。
            PlayerAvatarIcon.SetWithoutInvoke(prefService.GetInt(PlayerAvatarKey));
            PlayerAvatarIcon.Register(i => prefService.SetInt(PlayerAvatarKey, i));

            // 头像框：绑定存档写回。
            PlayerAvatarFrame.SetWithoutInvoke(prefService.GetInt(PlayerFrameKey));
            PlayerAvatarFrame.Register(i => prefService.SetInt(PlayerFrameKey, i));

            // 是否进入过资料页：用于新手引导或首进判断。
            ProfileEntered.SetWithoutInvoke(prefService.GetBool(ProfileEnteredKey));
            ProfileEntered.Register(i => prefService.SetBool(ProfileEnteredKey, i));
        }

        public Sprite GetAvatarIcon(int id)
        {
            return AvatarIconList[id];
        }

        public Sprite GetAvatarFrame(int id)
        {
            return AvatarFrameList[id];
        }

        public IEnumerable<int> GetAvatarIcons()
        {
            return AvatarIconList.Select((_, index) => index);
        }

        public IEnumerable<int> GetAvatarFrames()
        {
            return AvatarFrameList.Select((_, index) => index);
        }

        public int GetRandomAvatarIcon()
        {
            var count = AvatarIconList.Count;
            return Random.Range(0, count);
        }

        public int GetRandomAvatarFrame()
        {
            var count = AvatarFrameList.Count;
            return Random.Range(0, count);
        }
    }
}