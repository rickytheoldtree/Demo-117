using Demo_117.Services;
using Demo_117.UI.UIComponents.Profile;
using RicKit.RFramework;
using RicKit.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo_117.UI.UIPanels
{
    public class UIProfileEdit : PopUIPanel, ICanGetLocator<Entity>
    {
        [SerializeField] private AvatarBase avatar;
        [SerializeField] private ProfileTabButton iconTab, frameTab;
        [SerializeField] private ProfileAvatarIconItem avatarIconPrefab;
        [SerializeField] private ProfileAvatarFrameItem avatarFramePrefab;
        [SerializeField] private Transform avatarContent, frameContent;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private Button btnConfirm;
        
        private readonly BindableProperty<int> selectedTab = new();
        
        private IProfileService profileService;

        protected override void Awake()
        {
            base.Awake();
            this.TryGetService(out profileService);
            // 临时数据用于编辑预览，确认后再写回正式数据。
            profileService.PlayerAvatarIconTemp.SetWithoutInvoke(profileService.PlayerAvatarIcon.Value);
            profileService.PlayerAvatarIconTemp.RegisterAndInvoke(OnAvatarChanged);
            
            // 根据服务提供的资源列表生成图标项。
            foreach (var id in profileService.GetAvatarIcons())
            {
                var profileAvatarItem = Instantiate(avatarIconPrefab, avatarContent);
                profileAvatarItem.Init(id, profileService.PlayerAvatarIconTemp);
            }

            profileService.PlayerAvatarFrameTemp.SetWithoutInvoke(profileService.PlayerAvatarFrame.Value);
            profileService.PlayerAvatarFrameTemp.RegisterAndInvoke(OnFrameChanged);

            // 根据服务提供的资源列表生成头像框项。
            foreach (var id in profileService.GetAvatarFrames())
            {
                var profileAvatarItem = Instantiate(avatarFramePrefab, frameContent);
                profileAvatarItem.Init(id, profileService.PlayerAvatarFrameTemp);
            }
            
            // 共享选中索引，切换图标/头像框页签。
            iconTab.Init(0, selectedTab);
            frameTab.Init(1, selectedTab);
            
            profileService.PlayerNameTemp.SetWithoutInvoke(profileService.PlayerName.Value);
            inputName.text = profileService.PlayerNameTemp.Value;
            inputName.onEndEdit.AddListener(OnNameChanged);
            btnConfirm.onClick.AddListener(OnBtnConfirmClick);
            // 标记已进入过资料编辑页，用于引导或存档判断。
            profileService.ProfileEntered.Value = true;
        }

        private void OnDestroy()
        {
            // 解绑事件，避免对象销毁后仍被回调。
            profileService.PlayerAvatarIconTemp.UnRegister(OnAvatarChanged);
            profileService.PlayerAvatarFrameTemp.UnRegister(OnFrameChanged);
        }

        protected override void OnBackClick()
        {
            UI.Back(true);
        }

        private void OnNameChanged(string arg0)
        {
            // 输入中实时写入临时名字。
            profileService.PlayerNameTemp.Value = arg0;
        }

        private void OnAvatarChanged(int id)
        {
            avatar.SetIcon(id);
        }
        
        private void OnFrameChanged(int id)
        {
            avatar.SetFrame(id);
        }

        private void OnBtnConfirmClick()
        {
            // 点击确认后将临时数据提交到正式存档。
            profileService.PlayerName.Value = profileService.PlayerNameTemp.Value;
            profileService.PlayerAvatarIcon.Value = profileService.PlayerAvatarIconTemp.Value;
            profileService.PlayerAvatarFrame.Value = profileService.PlayerAvatarFrameTemp.Value;
            UI.Back(true);
        }
    }
}