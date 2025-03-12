using Unity.Mathematics;

namespace RTSGame
{
    public interface IUnitData
    {
        // Config Data    // 存一份基础数据   ； 其他的都是可变的数据。
        UnitType UnitType { get; }
        string Name { get; set;}
        int MaxHp { get; set;}
        int CurHp { get; set;} 
        int AtkNum { get; set;} // 攻击力
        int Cost { get; set;} // 造价
        int BuildTime { get; set;} // 建造时间
        
        // 生命恢复速度
        int HpRecoverySpeed { get; set;}
        int HpRecoveryValue { get; set; } 
        float3 Position { get; set; }
    }
    
    public interface IUnit : IBattleInit, IBattleTick, IBattleDispose, ICanUseSkill, ICanDef
    {
        long Guid { get; } 
        IUnitBehaviorCtrl BehaviorCtrl { get; }
        IUnitData UnitData { get; } 
        
        // 角色自己的一些方法
        void Move(UnityEngine.Vector3 destination);
        
        void TakeDamage(int damage);

        void SetPosition(float3 position);
        
        void SetGuid(long guid); 
    }
}
