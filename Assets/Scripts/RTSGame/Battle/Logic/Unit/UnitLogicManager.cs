using System.Collections.Generic;
using Unity.Mathematics;
using YuoTools;

namespace RTSGame
{
    public class UnitLogicManager : ManagerBase<UnitLogicManager>
    {
        private IUnitFactory _unitFactory;

        private long _globalGuid;
        // 持有所有的unit
        private List<IUnit> _allUnit;  
        
        #region 生命周期
        public override void Init()
        {
            base.Init();
            _globalGuid = 0;
            _unitFactory = new TestUnitFactory();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            foreach (var unit in _allUnit)
            {
                unit.Tick(deltaTime);
            }
        }
 
        public override void Dispose()
        {
            base.Dispose();
            foreach (var unit in _allUnit)
            {
                unit.Dispose();
            }
            _allUnit.Clear();
        }

        #endregion

        
        
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            RegisterEvent<I2LGameStart>(OnGameStart);
        }  
        
        /// <summary>
        /// 创建角色并初始化位置
        /// </summary> 
        public void CreateUnit(int unitType, float3 birthPos)
        {
            var character = _unitFactory.CreateUnit(unitType);
            character.SetGuid(_globalGuid++);

            var createEvent = L2VCreateUnit.Allocate();
            createEvent.Guid = character.Guid;
            createEvent.UnitData = character.UnitData;  
            createEvent.SendEvent();
            character.SetPosition(birthPos);
            AddUnitToList(character);
        }

        private void AddUnitToList(IUnit unit)
        {
            _allUnit.Add(unit);
            unit.Init();
        } 
        
        private void OnGameStart(I2LGameStart args)
        {
            $"测试开始:{args.StartName}".Log();
        }
    }
}
