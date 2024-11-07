using QFramework;
using UnityEngine;

namespace RTSGame.Battle.View
{
    /// <summary>
    /// 传入数据，创建单位显示层
    /// </summary>
    public interface IUnitViewFactory
    {
        GameObject CreateUnitView(IUnitData unitData); 
        GameObject CreateUnitView(IUnitData unitData, Vector3 position);
    }
}