namespace RTSGame.Battle.View
{
    /// <summary>
    /// 持有所有单位的显示层
    /// </summary>
    public class UnitViewManager : ManagerBase<UnitLogicManager>
    {   
        // 缺少工厂 + 资源加载模块
        // 工厂加对象池。
        
        
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
