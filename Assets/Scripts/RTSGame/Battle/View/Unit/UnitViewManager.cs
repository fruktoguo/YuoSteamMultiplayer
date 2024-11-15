using System.Collections.Generic;

namespace RTSGame.Battle.View
{
    /// <summary>
    /// 持有所有单位的显示层
    /// </summary>
    public class UnitViewManager : ManagerBase<UnitLogicManager>
    {   
        // 缺少工厂 + 资源加载模块
        // 工厂加对象池。
        private IUnitViewFactory _viewFactory;

        private List<UnitViewBase> _allUnit;
        private Dictionary<long,UnitViewBase> _unitDic;
        
        
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            RegisterEvent<L2VCreateUnit>(OnCreateUnit);
        }

        private void OnCreateUnit(L2VCreateUnit obj)
        { 
            AddUnit(_viewFactory.CreateUnitView(obj.UnitData),obj.Guid);
        }
        
        private void AddUnit(UnitViewBase unitView,long guid)
        {
            unitView.GUID = guid;
            _allUnit.Add(unitView);
            _unitDic.Add(guid,unitView);
            unitView.Init();
        }
        
        private void RemoveUnit(UnitViewBase unitView)
        {
            _allUnit.Remove(unitView);
            _unitDic.Remove(unitView.GUID);
            unitView.Dispose();
        }
        
    }
}
