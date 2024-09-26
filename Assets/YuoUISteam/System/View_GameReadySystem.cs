using DG.Tweening;
using FishNet.Managing;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_GameReadyComponent
    {
        public void Leave()
        {
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
            if (SteamAPIManager.Instance.CurrentLobby is {} lobby)
            {
                view.TextMeshProUGUI_Title.text = lobby.LobbyName;
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