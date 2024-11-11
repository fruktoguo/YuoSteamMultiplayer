using UnityEngine;

namespace RTSGame.Battle.View
{
    public class UnitViewFactoryBase : IUnitViewFactory
    {
        public void OnRecycled()
        { 
        }

        public bool IsRecycled { get; set; }
        public void Recycle2Cache()
        { 
        }

        public GameObject CreateUnitView(IUnitData unitData)
        { 
            return null;
        }

        public GameObject CreateUnitView(IUnitData unitData, Vector3 position)
        {
            return null;
        }
    }
}