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

        public void Join()
        {
            if (lobby.HasValue && CanJoin())
            {
                // ReflexHelper.LogAll(lobby.Value.Owner);
                // YuoNetworkManager.Instance.StartClient(lobby.Value.Owner.Id.Log());
            }
        }

        public bool CanJoin()
        {
            return true;
        }

        [Button]
        public void Refresh()
        {
            if (lobby.HasValue)
            {
                CSteamID owner = SteamMatchmakingHelper.GetLobbyOwner(lobby.Value);
                if (owner.m_SteamID == 0)
                {
                    lobby.Value.Refresh();
                    $"房间:{lobby.Value} 所有者:{owner}".Log();
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