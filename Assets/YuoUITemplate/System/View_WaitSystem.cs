using DG.Tweening;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_WaitComponent
    {
        public StringAction InfoGet;

        public static void Wait(StringAction showInfo)
        {
            var view = GetView();
            view.OpenView();
            view.InfoGet = showInfo;
        }

        public static void Close()
        {
            var view = GetView();
            view.CloseView();
        }
    }

    public class ViewWaitCreateSystem : YuoSystem<View_WaitComponent>, IUICreate
    {
        public override string Group => "UI/Wait";

        protected override void Run(View_WaitComponent view)
        {
            view.FindAll();
        }
    }

    public class ViewWaitOpenSystem : YuoSystem<View_WaitComponent>, IUIUpdate
    {
        public override string Group => "UI/Wait";

        protected override void Run(View_WaitComponent view)
        {
            view.TextMeshProUGUI_Text.text = view.InfoGet?.Invoke();
        }
    }
}