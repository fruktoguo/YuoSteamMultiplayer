namespace RTSGame
{
    public enum SkillType
    {
        Instant,        // 直接释放
        Targeted,       // 指向性
        AreaOfEffect,   // 范围效果
        Channeled,      // 持续施法
        Summon,         // 召唤
        Buff,           // 增益
        Debuff,         // 减益
        Movement        // 移动类
    }  
}