namespace RTSGame
{
    public enum UnitType
    {
        None,               // 无
        Character,          // 角色
        Building,           // 建筑
        Environment         // 环境
    }
    
    
    public enum CharacterType
    {
        None,               // 无
        Hero,               // 英雄
        Worker,             // 工人
        Soldier,            // 战士
        Archer,             // 弓箭手
        Mage                // 法师
    }
    
    public enum BuildingType
    {
        None,           // 无建筑 
        Farm,               // 农场  
        Barracks,           // 兵营 
        Tower,              // 防御塔 
    } 
    
    
    public enum UnitState
    {
        None,               // 无
        Idle,               // 待机
        Alert,              // 警戒
        Patrol,             // 巡逻
        Move,               // 移动
        AttackMove,         // 攻击移动
        Atk,                // 攻击
        Def,                // 防御 
        Build,              // 建造
        Repair,             // 修理
        Die,                // 死亡
        HoldPosition,       // 保持位置
        Flee,               // 逃跑
        Heal,               // 治疗
        GatherResources,    // 采集资源
        Transport,          // 运输
        Unload,             // 卸载
        Charge,             // 冲锋
        Stunned             // 眩晕
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}