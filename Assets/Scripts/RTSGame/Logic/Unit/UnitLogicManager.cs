using YuoTools;

namespace RTSGame
{
    public class UnitLogicManager : ManagerBase<UnitLogicManager>
    {
        private IUnitFactory _unitFactory;

        public override void Init()
        {
            base.Init();
            _unitFactory = new TestUnitFactory();
        }


        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            RegisterEvent<I2LGameStart>(OnGameStart);
        }
        
        
        
        
        
        
        private void OnGameStart(I2LGameStart args)
        {
            $"测试开始:{args.StartName}".Log();
        }
    }
}
