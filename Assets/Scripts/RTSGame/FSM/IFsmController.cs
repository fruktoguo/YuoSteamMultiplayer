namespace RTSGame
{
    public interface IFsmController<T>
    {
        void ChangeState(T stateType);

        void AddState(T stateType,IFsmState state);

        void RemoveState(T stateType);

        void UpdateState(float deltaTime); 
    }
}
