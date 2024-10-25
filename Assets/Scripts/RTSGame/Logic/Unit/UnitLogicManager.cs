using Unity.Mathematics;
using YuoTools;

namespace RTSGame
{
    public class UnitLogicManager : ManagerBase<UnitLogicManager>
    {
        private IUnitFactory _unitFactory;

        private long _globalGuid;
        
        public override void Init()
        {
            base.Init();
            _globalGuid = 0;
            _unitFactory = new TestUnitFactory();
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            RegisterEvent<I2LGameStart>(OnGameStart);
        }
         
        
        
        /// <summary>
        /// 创建角色并初始化位置
        /// </summary> 
        public void CreateCharacter(CharacterType characterType, float3 birthPos)
        {
            var character = _unitFactory.CreateCharacter(characterType);
            character.SetGuid(_globalGuid++);

            var createEvent = L2VCreateUnit.Allocate();
            createEvent.Guid = character.Guid;
            createEvent.UnitData = character.UnitData;  
            createEvent.SendEvent(); 
            
            character.SetPosition(birthPos);
        }
        
        
        
        private void OnGameStart(I2LGameStart args)
        {
            $"测试开始:{args.StartName}".Log();
        }
    }
}
