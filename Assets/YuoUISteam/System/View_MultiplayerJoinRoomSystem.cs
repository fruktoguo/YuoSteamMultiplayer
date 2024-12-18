﻿using System.Collections.Generic;
using DG.Tweening;
using SteamAPI.SteamHelper;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_MultiplayerJoinRoomComponent
    {
        public List<View_RoomItemComponent> RoomItems = new();
    }

    public class ViewMultiplayerJoinRoomCreateSystem : YuoSystem<View_MultiplayerJoinRoomComponent>, IUICreate
    {
        public override string Group => "UI/MultiplayerJoinRoom";

        protected override void Run(View_MultiplayerJoinRoomComponent view)
        {
            view.FindAll();
            //关闭窗口的事件注册,名字不同请自行更
            view.Button_Close.SetUICloseAndOpen(view.ViewName, ViewType.MultiplayerMenu);
            // view.Button_Mask.SetUIClose(view.ViewName);
        }
    }

    public class ViewMultiplayerJoinRoomOpenSystem : YuoSystem<View_MultiplayerJoinRoomComponent>, IUIOpen
    {
        public override string Group => "UI/MultiplayerJoinRoom";

        protected override async void Run(View_MultiplayerJoinRoomComponent view)
        {
            SteamLobbyHelper.SetLobbyFindFilter();
            var lobbies = await SteamLobbyHelper.FindLobbyListAsync();
            view.RoomItems.DestroyAll();
            if (lobbies is { Length: > 0 })
            {
                foreach (var lobby in lobbies)
                {
                    var lobbyItem = view.AddChildAndInstantiate(view.Child_RoomItem);
                    lobbyItem.TextMeshProUGUI_Name.text = lobby.LobbyName;
                    lobbyItem.Toggle_UsePassword.isOn = lobby.HasLobbyPassword();
                    lobbyItem.TextMeshProUGUI_Players.text = $"人数:{lobby.MemberCount}/{lobby.MaxMemberCount}";
                    lobbyItem.lobby = lobby;
                    lobbyItem.AddComponent<UIActiveComponent>();
                    view.RoomItems.Add(lobbyItem);
                }
            }
        }
    }

    public class ViewMultiplayerJoinRoomCloseSystem : YuoSystem<View_MultiplayerJoinRoomComponent>, IUIClose
    {
        public override string Group => "UI/MultiplayerJoinRoom";

        protected override void Run(View_MultiplayerJoinRoomComponent view)
        {
            view.RoomItems.DestroyAll();
        }
    }

    public class
        ViewMultiplayerJoinRoomOpenAnimaSystem : YuoSystem<View_MultiplayerJoinRoomComponent, UIAnimaComponent>, IUIOpen
    {
        public override string Group => "UI/MultiplayerJoinRoom";

        protected override void Run(View_MultiplayerJoinRoomComponent view, UIAnimaComponent anima)
        {
            view.Button_Mask.image.SetColorA(0);

            view.Button_Mask.image.DOFade(0.6f, anima.AnimaDuration);
        }
    }

    public class
        ViewMultiplayerJoinRoomCloseAnimaSystem : YuoSystem<View_MultiplayerJoinRoomComponent, UIAnimaComponent>,
        IUIClose
    {
        public override string Group => "UI/MultiplayerJoinRoom";

        protected override void Run(View_MultiplayerJoinRoomComponent view, UIAnimaComponent anima)
        {
            view.Button_Mask.image.DOFade(0f, anima.AnimaDuration);
        }
    }
}