using System.Collections.Generic;
using DG.Tweening;
using Steamworks;
using UnityEngine;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_LobbyComponent
    {
        public List<View_LobbyItemComponent> lobbyItemList = new();

        void Init()
        {
        }

        public void CreateLobby()
        {
            SteamLobbyManager.Instance.CreateLobby();
        }

        public void FindLobbies()
        {
            SteamLobbyManager.Instance.FindAllLobbies();
        }

        public void CloseLobby()
        {
            ScrollRect_LobbyScroll.Hide();
            VerticalLayoutGroup_SelectLobby.Show();
        }

        public void UpdateLobbyList(string lobbyListString)
        {
            // 清空现有的大厅列表
            foreach (var viewLobbyItemComponent in lobbyItemList)
            {
                viewLobbyItemComponent.Entity.Destroy();
            }

            lobbyItemList.Clear();

            // 获取大厅列表
            List<CSteamID> lobbies = SteamLobbyManager.Instance.GetLobbyList();
            foreach (CSteamID lobbyID in lobbies)
            {
                var item = AddChildAndInstantiate(Child_LobbyItem);
                item.lobbyID = lobbyID;

                item.Init();
                item.rectTransform.SetParent(ContentSizeFitter_Content.transform);
                lobbyItemList.Add(item);
            }

            ScrollRect_LobbyScroll.Show();
            VerticalLayoutGroup_SelectLobby.Hide();
        }

        public void JoinLobby(CSteamID lobbyID)
        {
            SteamLobbyManager.Instance.JoinLobby(lobbyID);
        }

        public void OnLobbyCreated(bool success, string message)
        {
            if (success)
            {
            }
        }

        public void OnLobbyEntered(bool success, string message)
        {
            // Debug.Log(message);
            if (success)
            {
                this.CloseAndOpenView<View_RoomComponent>();
            }
        }
    }

    public class ViewLobbyCreateSystem : YuoSystem<View_LobbyComponent>, IUICreate
    {
        public override string Group => "UI/Lobby";

        protected override async void Run(View_LobbyComponent view)
        {
            view.FindAll();
            //关闭窗口的事件注册,名字不同请自行更
            view.Button_Close.SetUIClose(view.ViewName);

            SteamLobbyManager.OnLobbyListUpdated += view.UpdateLobbyList;
            SteamLobbyManager.OnLobbyCreatedCallback += view.OnLobbyCreated;
            SteamLobbyManager.OnLobbyEnteredCallback += view.OnLobbyEntered;

            view.Button_Create.SetBtnClick(view.CreateLobby);
            view.Button_Find.SetBtnClick(view.FindLobbies);
            view.Button_LobbyRefresh.SetBtnClick(view.FindLobbies);
            view.Button_BackSelect.SetBtnClick(view.CloseLobby);

            await SteamNetworkManager.Instance.CheckNATStatus(x => view.TextMeshProUGUI_Tip.text = x);
        }
    }

    public class ViewLobbyOpenSystem : YuoSystem<View_LobbyComponent>, IUIOpen
    {
        public override string Group => "UI/Lobby";

        protected override void Run(View_LobbyComponent view)
        {
        }
    }

    public class ViewLobbyCloseSystem : YuoSystem<View_LobbyComponent>, IUIClose
    {
        public override string Group => "UI/Lobby";

        protected override void Run(View_LobbyComponent view)
        {
        }
    }
}