namespace RTSGame
{
    public interface IUnitData
    { 
        string Name { get; }
        int MaxHp { get; protected set; }
        int CurHp { get; protected set; } 
        int AtkNum { get; protected set; } // 攻击力
        int Cost { get; } // 造价
        int BuildTime { get; } // 建造时间
        
        // 生命恢复速度
        int HpRecoverySpeed { get; protected set; }
        int HpRecoveryValue { get; protected set; }
        
        /// <summary>
        /// 受到伤害 
        /// </summary>
        public void TakeDamage();
        
        /// <summary>
        /// 治疗 / 维修
        /// </summary>
        public void Heal();
    }
    
    public interface IUnit
    {
        long Guid { get; }
        UnitType UnitType { get; }
        UnitState UnitState { get; }
        IUnitData UnitData { get; } 
        void SetState(UnitState newState);  //TODO :考虑搞成状态机
        void TakeDamage(int damage);
    }
}
