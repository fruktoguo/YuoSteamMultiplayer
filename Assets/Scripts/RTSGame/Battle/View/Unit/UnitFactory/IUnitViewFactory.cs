using QFramework;
using UnityEngine;

namespace RTSGame.Battle.View
{
    /// <summary>
    /// 传入数据，创建单位显示层
    /// </summary>
    public interface IUnitViewFactory
    {
        UnitViewBase CreateUnitView(IUnitData unitData); 
        UnitViewBase CreateUnitView(IUnitData unitData, Vector3 position);
    }
}