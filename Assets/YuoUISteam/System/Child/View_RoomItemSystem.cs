using Sirenix.OdinInspector;
using SteamAPI.SteamHelper;
using Steamworks;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_RoomItemComponent
    {
        public Lobby? lobby;

        public async void Join()
        {
            if (lobby.HasValue && CanJoin())
            {
                var roomEnter = await lobby.Value.Join();
                if (roomEnter == Lobby.RoomEnter.Success)
                {
                    View_MultiplayerJoinRoomComponent.GetView().CloseView();
                }
            }
        }

        public bool CanJoin()
        {
            return LoadOwnerOver;
        }

        public bool LoadOwnerOver;

        [Button]
        public void Refresh()
        {
            if (lobby.HasValue)
            {
                CSteamID owner = SteamMatchmakingHelper.GetLobbyOwner(lobby.Value);
                LoadOwnerOver = owner.m_SteamID != 0;
                Button_Join.interactable = LoadOwnerOver;
                Image_Mask.gameObject.SetActive(!LoadOwnerOver);
                if (!LoadOwnerOver)
                {
                    lobby.Value.Refresh();
                    // $"房间:{lobby.Value} 所有者:{owner}".Log();
                }
            }
        }
    }

    public class ViewRoomItemCreateSystem : YuoSystem<View_RoomItemComponent>, IUICreate
    {
        public override string Group => "UI/RoomItem";

        protected override void Run(View_RoomItemComponent view)
        {
            view.FindAll();

            view.Button_Join.SetBtnClick(view.Join);
        }
    }

    public class ViewRoomItemOpenSystem : YuoSystem<View_RoomItemComponent>, IUIOpen
    {
        public override string Group => "UI/RoomItem";

        protected override void Run(View_RoomItemComponent view)
        {
            view.Refresh();
        }
    }

    public class ViewRoomItemCloseSystem : YuoSystem<View_RoomItemComponent>, IUIClose
    {
        public override string Group => "UI/RoomItem";

        protected override void Run(View_RoomItemComponent view)
        {
        }
    }

    public class ViewRoomItemUpdateSystem : YuoSystem<View_RoomItemComponent, UIActiveComponent>, IUpdateEverySecond
    {
        protected override void Run(View_RoomItemComponent view, UIActiveComponent active)
        {
            view.Refresh();
        }
    }
}