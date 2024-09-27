using DG.Tweening;
using SteamAPI.SteamHelper;
using Steamworks;
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
	}
	public class ViewPlayerReadyCreateSystem :YuoSystem<View_PlayerReadyComponent>, IUICreate
	{
		public override string Group =>"UI/PlayerReady";

		protected override void Run(View_PlayerReadyComponent view)
		{
			view.FindAll();
		}
	}
	public class ViewPlayerReadyOpenSystem :YuoSystem<View_PlayerReadyComponent>, IUIOpen
	{
		public override string Group =>"UI/PlayerReady";

		protected override void Run(View_PlayerReadyComponent view)
		{
		}
	}
	public class ViewPlayerReadyCloseSystem :YuoSystem<View_PlayerReadyComponent>, IUIClose
	{
		public override string Group =>"UI/PlayerReady";

		protected override void Run(View_PlayerReadyComponent view)
		{
		}
	}
}
