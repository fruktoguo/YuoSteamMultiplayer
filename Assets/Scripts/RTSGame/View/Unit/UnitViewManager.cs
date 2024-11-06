namespace RTSGame
{
    /// <summary>
    /// 持有所有单位的显示层
    /// </summary>
    public class UnitViewManager : ManagerBase<UnitLogicManager>
    { 
        
        
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            RegisterEvent<L2VCreateUnit>(OnCreateUnit);
        }

        private void OnCreateUnit(L2VCreateUnit obj)
        {
            
        }
    }
}
