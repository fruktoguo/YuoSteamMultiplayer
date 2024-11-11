namespace RTSGame
{
    public interface IFsmState
    {
        void OnInit();

        void OnEnter();

        void OnLeave();

        void OnUpdate(float deltaTime);
    }
}
