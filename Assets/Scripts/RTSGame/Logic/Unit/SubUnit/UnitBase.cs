using Unity.Mathematics;

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

        protected UnitData(UnitType unitType, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue)
        {
            UnitType = unitType;
            Name = name;
            MaxHp = maxHp;
            CurHp = curHp;
            AtkNum = atkNum;
            Cost = cost;
            BuildTime = buildTime;
            HpRecoverySpeed = hpRecoverySpeed;
            HpRecoveryValue = hpRecoveryValue;
        } 
    }
    
    
    public abstract class UnitBase : IUnit
    {
        public long Guid { get; private set; } 
        public UnitState UnitState { get; }
        public IUnitData UnitData { get; }
        
        // --------
        public float3 Position { get; protected set; }
        
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

        public void SetPosition(float3 position)
        {
            Position = position;
            L2VSetUnitPosition.SendEvent(Guid, position);
        }
        
        public void SetGuid(long guid)
        {
            Guid = guid;
        }
    }
}
