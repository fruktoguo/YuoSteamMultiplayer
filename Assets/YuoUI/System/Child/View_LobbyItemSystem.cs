using DG.Tweening;
using Steamworks;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_LobbyItemComponent
    {
        public CSteamID lobbyID;

        public void Init()
        {
            Refresh();
        }

        public void Refresh()
        {
            TextMeshProUGUI_Text.text =
                $"大厅ID: {lobbyID} 人数: {SteamLobbyManager.Instance.GetLobbyPlayerCount(lobbyID)} / {SteamLobbyManager.Instance.GetLobbyMaxPlayerCount(lobbyID)}";

            // SteamLobbyManager.Instance.GetUserIP(lobbyID);
        }
    }

    public class ViewLobbyItemCreateSystem : YuoSystem<View_LobbyItemComponent>, IUICreate
    {
        public override string Group => "UI/LobbyItem";

        protected override void Run(View_LobbyItemComponent view)
        {
            view.FindAll();

            view.Button_Joint.SetBtnClick(() => { SteamLobbyManager.Instance.JoinLobby(view.lobbyID); });
        }
    }

    public class ViewLobbyItemOpenSystem : YuoSystem<View_LobbyItemComponent>, IUIOpen
    {
        public override string Group => "UI/LobbyItem";

        protected override void Run(View_LobbyItemComponent view)
        {
        }
    }

    public class ViewLobbyItemCloseSystem : YuoSystem<View_LobbyItemComponent>, IUIClose
    {
        public override string Group => "UI/LobbyItem";

        protected override void Run(View_LobbyItemComponent view)
        {
        }
    }
}