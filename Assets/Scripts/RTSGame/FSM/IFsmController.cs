namespace JHGame.Tools.FSM
{
    public interface IFsmController<T>
    {
        void ChangeState(T stateType);

        void AddState(T stateType,IFsmState state);

        void RemoveState(T stateType);

        void UpdateState();
    }
}
