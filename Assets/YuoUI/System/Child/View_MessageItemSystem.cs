using DG.Tweening;
using YuoTools.Extend.Helper;
using YuoTools.Main.Ecs;

namespace YuoTools.UI
{
    public partial class View_MessageItemComponent
    {
    }

    public class ViewMessageItemCreateSystem : YuoSystem<View_MessageItemComponent>, IUICreate
    {
        public override string Group => "UI/MessageItem";

        protected override void Run(View_MessageItemComponent view)
        {
            view.FindAll();
        }
    }

    public class ViewMessageItemOpenSystem : YuoSystem<View_MessageItemComponent>, IUIOpen
    {
        public override string Group => "UI/MessageItem";

        protected override void Run(View_MessageItemComponent view)
        {
        }
    }

    public class ViewMessageItemCloseSystem : YuoSystem<View_MessageItemComponent>, IUIClose
    {
        public override string Group => "UI/MessageItem";

        protected override void Run(View_MessageItemComponent view)
        {
        }
    }
}