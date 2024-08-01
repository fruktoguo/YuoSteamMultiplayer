using System.Collections.Generic;
using DG.Tweening;
using Steamworks;
using UnityEngine;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_RoomComponent
    {
        private List<View_MessageItemComponent> messageItemList = new();

        public void SendMessage()
        {
            string message = TMP_InputField_Input.text;
            if (!string.IsNullOrEmpty(message))
            {
                // 发送消息给所有成员
                List<CSteamID> lobbyMembers = SteamLobbyManager.Instance.GetPlayerList();
                foreach (CSteamID memberID in lobbyMembers)
                {
                    Debug.Log($"发送消息给成员ID: {memberID}");
                    SteamLobbyManager.Instance.SendP2PRequest(memberID, message);
                }

                TMP_InputField_Input.text = "";
            }
        }

        public void EnterRoom()
        {
            // 清空现有的消息列表
            foreach (var message in messageItemList)
            {
                message.Entity.Destroy();
            }
        }

        public void BackToLobby()
        {
            // 调用退出大厅的方法
            SteamLobbyManager.Instance.ExitLobby();
        }

        public void AddMessage(string message)
        {
            var messageItem = AddChildAndInstantiate(Child_MessageItem);
            messageItem.TextMeshProUGUI_Text.text = message;
            messageItemList.Add(messageItem);
        }

        public void UpdateChatRoom(string chatRoom)
        {
            // 解析并显示新的消息
            string[] messages = chatRoom.Split('\n');
            foreach (string msg in messages)
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    AddMessage(msg);
                }
            }
        }
    }

    public class ViewRoomCreateSystem : YuoSystem<View_RoomComponent>, IUICreate
    {
        public override string Group => "UI/Room";

        protected override void Run(View_RoomComponent view)
        {
            view.FindAll();
            //关闭窗口的事件注册,名字不同请自行更
            view.Button_Close.SetUIClose(view.ViewName);

            SteamLobbyManager.OnChatRoomUpdated += view.UpdateChatRoom;
            SteamLobbyManager.OnLobbyExitedCallback += view.CloseAndOpenView<View_LobbyComponent>;

            view.Button_Send.SetBtnClick(view.SendMessage);
            view.Button_Close.SetBtnClick(view.BackToLobby);
            view.TMP_InputField_Input.onSubmit.AddListener(x => view.SendMessage());
        }
    }

    public class ViewRoomOpenSystem : YuoSystem<View_RoomComponent>, IUIOpen
    {
        public override string Group => "UI/Room";

        protected override void Run(View_RoomComponent view)
        {
            view.EnterRoom();
        }
    }

    public class ViewRoomCloseSystem : YuoSystem<View_RoomComponent>, IUIClose
    {
        public override string Group => "UI/Room";

        protected override void Run(View_RoomComponent view)
        {
        }
    }
}