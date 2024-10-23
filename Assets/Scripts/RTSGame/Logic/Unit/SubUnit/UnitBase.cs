namespace RTSGame
{
    public abstract class UnitData : IUnitData
    {
        public UnitType UnitType { get; protected set; } 
        public string Name { get; protected set; } 
        public int MaxHp { get; protected set; }
        public int CurHp { get; protected set; }
        public int AtkNum { get; protected set; }
        public int Cost { get; protected set; }
        public int BuildTime { get; protected set; }
        public int HpRecoverySpeed { get; protected set; }
        public int HpRecoveryValue { get; protected set; }
  
        public void TakeDamage()
        {
            
        }

        public void Heal()
        { 
        } 
    }
    
    
    public abstract class UnitBase : IUnit
    {
        public long Guid { get; } 
        public UnitState UnitState { get; }
        public IUnitData UnitData { get; }
        
        public UnitBase(IUnitData data)
        { 
            UnitData = data;
            UnitState = UnitState.Idle; // 默认状态
        }
        
        // 这个state只是标记状态，并不是状态机，  状态机应该由单个角色去添加
        public void SetState(UnitState newState)
        {
            
        }

        public void TakeDamage(int damage)
        {
            
        } 
    }
}
