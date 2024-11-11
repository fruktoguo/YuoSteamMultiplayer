namespace RTSGame.UnitBehavior
{
    public abstract class UnitStateBase : IFsmState
    {
        private UnitBehaviorCtrlBase CtrlBase { get; set; }
        
        public void SetCtrl(UnitBehaviorCtrlBase ctrlBase)
        {
            CtrlBase = ctrlBase;
        }
        
        public virtual void OnInit()
        { 
        }

        public virtual void OnEnter()
        { 
        }

        public virtual void OnLeave()
        { 
        }

        public virtual void OnUpdate(float deltaTime)
        { 
        }
    }
}