using System.Collections.Generic;
using DG.Tweening;
using FishNet;
using FishNet.Managing;
using SteamAPI.SteamHelper;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_GameReadyComponent
    {
        public void Leave()
        { 
            // 关闭页面 
            this.CloseView(); 
            // if (SteamAPIManager.Instance.CurrentLobby != null)
            // {
            //     SteamMatchmakingHelper.LeaveLobby(SteamAPIManager.Instance.CurrentLobby.Value);
            //     this.CloseAndOpenView<View_MultiplayerMenuComponent>();
            // } 
        }

        public void PlayerEnter(NetPlayerComponent component)
        {
            if (playerViews.ContainsKey(component)) return;
            View_PlayerReadyComponent player = AddChildAndInstantiate(Child_PlayerReady);
            player.steamID = component.steamID;
            player.TextMeshProUGUI_PlayerName.text = component.Entity.EntityName;
            player.UpdateAvatar();
            playerViews.Add(component, player);
        }

        Dictionary<NetPlayerComponent, View_PlayerReadyComponent> playerViews = new();

        public void PlayerExit(NetPlayerComponent component)
        {
            View_PlayerReadyComponent player = playerViews[component];
            player.Entity.Destroy();
            playerViews.Remove(component);
        }
    }

    public class NetPlayerComponentStartSystem : YuoSystem<NetPlayerComponent>, INetPlayerAwake, IDestroy
    {
        protected override void Run(NetPlayerComponent component)
        {
            var readyView = View_GameReadyComponent.GetView();
            if (RunType == typeof(INetPlayerAwake))
            {
                readyView.PlayerEnter(component);
            }

            if (RunType == typeof(IDestroy))
            {
                readyView.PlayerExit(component);
            }
        }
    }

    public class ViewGameReadyCreateSystem : YuoSystem<View_GameReadyComponent>, IUICreate
    {
        public override string Group => "UI/GameReady";

        protected override void Run(View_GameReadyComponent view)
        {
            view.FindAll();
            //关闭窗口的事件注册,名字不同请自行更
            view.Button_Close.SetBtnClick(view.Leave);
        }
    }

    public class ViewGameReadyOpenSystem : YuoSystem<View_GameReadyComponent>, IUIOpen
    {
        public override string Group => "UI/GameReady";

        protected override void Run(View_GameReadyComponent view)
        {
            if (SteamAPIManager.Instance.CurrentLobby is { } lobby)
            {
                view.TextMeshProUGUI_Title.text = lobby.LobbyName;
                view.Button_StartGame.gameObject.SetActive(InstanceFinder.IsServerStarted);
            }
        }
    }

    public class ViewGameReadyCloseSystem : YuoSystem<View_GameReadyComponent>, IUIClose
    {
        public override string Group => "UI/GameReady";

        protected override void Run(View_GameReadyComponent view)
        {
        }
    }
}