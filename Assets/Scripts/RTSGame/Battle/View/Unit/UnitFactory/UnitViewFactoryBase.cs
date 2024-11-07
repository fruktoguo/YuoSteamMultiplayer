using UnityEngine;

namespace RTSGame.Battle.View
{
    public class UnitViewFactoryBase : IUnitViewFactory
    {
        public void OnRecycled()
        {
            throw new System.NotImplementedException();
        }

        public bool IsRecycled { get; set; }
        public void Recycle2Cache()
        {
            throw new System.NotImplementedException();
        }

        public GameObject CreateUnitView(IUnitData unitData)
        {
            throw new System.NotImplementedException();
        }

        public GameObject CreateUnitView(IUnitData unitData, Vector3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}