namespace JHGame.Tools.FSM
{
    public interface IFsmState
    {
        void OnInit();

        void OnEnter();

        void OnLeave();

        void OnUpdate();
    }
}
