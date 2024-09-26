using DG.Tweening;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;
namespace YuoTools.UI
{
	public partial class View_PlayerReadyComponent
	{
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
