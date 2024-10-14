namespace RTSGame
{
    public abstract class UnitData : IUnitData
    {
        public string Name { get; }
        int IUnitData.MaxHp { get; set; }
        int IUnitData.CurHp { get; set; } 
        int IUnitData.AtkNum { get; set; } 
        public int Cost { get; }
        public int BuildTime { get; } 
        int IUnitData.HpRecoverySpeed { get; set; } 
        int IUnitData.HpRecoveryValue { get; set; }

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
        public UnitType UnitType { get; }
        public UnitState UnitState { get; }
        public IUnitData UnitData { get; }
         
        // 这个state只是标记状态，并不是状态机，  状态机应该由单个角色去添加
        public void SetState(UnitState newState)
        {
             
        }

        public void TakeDamage(int damage)
        {
            
        } 
    }
}