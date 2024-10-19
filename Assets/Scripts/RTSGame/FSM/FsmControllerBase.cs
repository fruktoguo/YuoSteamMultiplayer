using System;
using System.Collections.Generic;

namespace JHGame.Tools.FSM
{
    public abstract class FsmControllerBase<T> : IFsmController<T> where T: Enum
    {
        private IFsmState curState;
        private Dictionary<T, IFsmState> stateDic = new();
        
        public void ChangeState(T stateType)
        {
            if (stateDic.TryGetValue(stateType, out var value))
            {
                curState?.OnLeave();
                curState = value;
                curState.OnEnter();
            }
        }

        public void AddState(T stateType,IFsmState state)
        {
            if (!stateDic.ContainsKey(stateType))
            {
                stateDic.Add(stateType,state);
                state.OnInit();
            }
        }

        public void RemoveState(T stateType)
        {
            if (stateDic.ContainsKey(stateType))
            {
                stateDic.Remove(stateType);
            }
        }
 
        public void UpdateState()
        {
            curState?.OnUpdate();
        }
    }
}