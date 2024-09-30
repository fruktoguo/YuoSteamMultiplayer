using DG.Tweening;
using SteamAPI.SteamHelper;
using Steamworks;
using UnityEngine;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_PlayerReadyComponent
    {
        public CSteamID steamID;

        public void UpdateAvatar()
        {
            if (steamID.m_SteamID == 0)
            {
                $"玩家数据未加载".Log();
                return;
            }

            var avatar = SteamFriendsHelper.GetLargeFriendAvatar(steamID);
            RawImage_Head.texture = SteamUtilsHelper.GetImage(avatar);
        }

        public bool IsReady;

        public void OnClick_Ready()
        {
            IsReady = !IsReady;
            player.ready.SetValue(IsReady);
            Button_Ready.DisableInteractable(0.5f);
        }

        public void SetReady(bool isReady)
        {
            Button_Ready.image.color = IsReady ? Color.green : Color.red;
            TextMeshProUGUI_ReadyText.text = IsReady ? "已准备" : "未准备";
        }

        public NetPlayerComponent player;
    }

    public class ViewPlayerReadyCreateSystem : YuoSystem<View_PlayerReadyComponent>, IUICreate
    {
        public override string Group => "UI/PlayerReady";

        protected override async void Run(View_PlayerReadyComponent view)
        {
            view.FindAll();
            await YuoWait.WaitUntilAsync(() => view.player != null);
            view.Button_Ready.interactable = view.player.player.IsOwner;
            if (view.player.player.IsOwner)
            {
                view.Button_Ready.SetBtnClick(view.OnClick_Ready);
            }

            view.player.ready.onReadyChange.AddListener(view.SetReady);
        }
    }

    public class ViewPlayerReadyOpenSystem : YuoSystem<View_PlayerReadyComponent>, IUIOpen
    {
        public override string Group => "UI/PlayerReady";

        protected override void Run(View_PlayerReadyComponent view)
        {
        }
    }

    public class ViewPlayerReadyCloseSystem : YuoSystem<View_PlayerReadyComponent>, IUIClose
    {
        public override string Group => "UI/PlayerReady";

        protected override void Run(View_PlayerReadyComponent view)
        {
        }
    }
}