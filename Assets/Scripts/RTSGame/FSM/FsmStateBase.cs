namespace JHGame.Tools.FSM
{
    public abstract class FsmStateBase<T> : IFsmState
    {
        public virtual void OnInit(){}

        public virtual void OnEnter(){}

        public virtual void OnLeave(){}

        public virtual void OnUpdate(){}
    }
}
