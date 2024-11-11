using Unity.Mathematics;

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
    }
    
    public interface IUnit : IBattleInit, IBattleTick, IBattleDispose
    {
        long Guid { get; } 
        IUnitBehaviorCtrl BehaviorCtrl { get; }
        IUnitData UnitData { get; } 
        void TakeDamage(int damage);

        void SetPosition(float3 position);
        
        void SetGuid(long guid); 
    }
}
