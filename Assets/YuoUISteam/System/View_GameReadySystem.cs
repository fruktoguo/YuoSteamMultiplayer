using System.Collections.Generic;
using DG.Tweening;
using FishNet;
using FishNet.Managing;
using Game;
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
            if (SteamAPIManager.Instance.CurrentLobby != null)
            {
                SteamMatchmakingHelper.LeaveLobby(SteamAPIManager.Instance.CurrentLobby.Value);
                this.OpenView<View_MultiplayerMenuComponent>();
            }
        }

        public void PlayerEnter(NetPlayerComponent component)
        {
            if (playerViews.ContainsKey(component)) return;
            View_PlayerReadyComponent player = AddChildAndInstantiate(Child_PlayerReady);
            player.player = component;
            player.steamID = component.steamID;
            player.TextMeshProUGUI_PlayerName.text = component.Entity.EntityName;
            player.UpdateAvatar();
            playerViews.Add(component, player);
        }

        public Dictionary<NetPlayerComponent, View_PlayerReadyComponent> playerViews = new();

        public void PlayerExit(NetPlayerComponent component)
        {
            View_PlayerReadyComponent player = playerViews[component];
            player.Entity.Destroy();
            playerViews.Remove(component);
        }

        public void StartGame()
        {
            foreach (var viewPlayerReadyComponent in playerViews.Values)
            {
                if (!viewPlayerReadyComponent.IsReady)
                {
                    return;
                }
            }

            GameManager.Instance.StartGame();
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

            view.Button_StartGame.SetBtnClick(view.StartGame);
        }
    }

    public class ViewGameReadyOpenSystem : YuoSystem<View_GameReadyComponent>, IUIOpen
    {
        public override string Group => "UI/GameReady";

        protected override async void Run(View_GameReadyComponent view)
        {
            await YuoWait.WaitUntilAsync(() => SteamAPIManager.Instance.CurrentLobby != null);
            if (SteamAPIManager.Instance.CurrentLobby is { } lobby)
            {
                view.TextMeshProUGUI_Title.text = lobby.LobbyName;
                view.Button_StartGame.gameObject.SetActive(InstanceFinder.IsServerStarted);
            }

            GameManager.Instance.OnGameStart.AddListener(view.CloseView);
        }
    }

    public class ViewGameReadyCloseSystem : YuoSystem<View_GameReadyComponent>, IUIClose
    {
        public override string Group => "UI/GameReady";

        protected override void Run(View_GameReadyComponent view)
        {
            GameManager.Instance.OnGameStart.RemoveListener(view.CloseView);
        }
    }

    public class ViewGameReadyUpdateSystem : YuoSystem<View_GameReadyComponent>, IUIUpdate
    {
        public override string Group => "UI/GameReady";

        protected override void Run(View_GameReadyComponent view)
        {
            bool isReady = true;
            foreach (var item in view.playerViews.Values)
            {
                isReady &= item.IsReady;
            }

            view.Button_StartGame.interactable = isReady;
        }
    }
}