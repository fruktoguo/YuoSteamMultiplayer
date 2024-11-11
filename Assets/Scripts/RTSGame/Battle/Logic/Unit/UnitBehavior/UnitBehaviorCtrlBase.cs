using System.Collections.Generic;

namespace RTSGame.UnitBehavior
{
    /// <summary>
    /// 管理Unit行为状态的基类
    /// </summary>
    public abstract class UnitBehaviorCtrlBase : IUnitBehaviorCtrl
    {
        protected IFsmState CurState;
        protected Dictionary<UnitState, UnitStateBase> StateDic = new(); 
        public UnitBase Unit { get; set; } 
        
        public UnitBehaviorCtrlBase(UnitBase unit)
        {
            Unit = unit;
        }
        
        public void ChangeState(UnitState stateType)
        {
            if (StateDic.TryGetValue(stateType, out var value))
            {
                CurState?.OnLeave();
                CurState = value;
                CurState.OnEnter();
            }
        }
 
        public void AddState(UnitState stateType,IFsmState state)
        {
            if (state is UnitStateBase unitState)
            {
                if (StateDic.TryAdd(stateType, unitState))
                {
                    unitState.OnInit();
                    unitState.SetCtrl(this);
                }
            } 
        }

        public void RemoveState(UnitState stateType)
        {
            if (StateDic.ContainsKey(stateType))
            {
                StateDic.Remove(stateType);
            }
        }
 
        public void UpdateState(float deltaTime)
        {
            CurState?.OnUpdate(deltaTime);
        }
    }
}