namespace RTSGame
{
    public interface IUnitData
    { 
        UnitType UnitType { get; }
        string Name { get;}
        int MaxHp { get; }
        int CurHp { get; } 
        int AtkNum { get; } // 攻击力
        int Cost { get; } // 造价
        int BuildTime { get; } // 建造时间
        
        // 生命恢复速度
        int HpRecoverySpeed { get; }
        int HpRecoveryValue { get; }
        
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
        UnitState UnitState { get; }
        IUnitData UnitData { get; } 
        void SetState(UnitState newState);  //TODO :考虑搞成状态机
        void TakeDamage(int damage);
    }
}
