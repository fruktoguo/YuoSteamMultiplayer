namespace RTSGame
{
    public interface ICharacterData : IUnitData
    {
        // 角色自己的数据, 之后都走配置表
        public float MovementSpeed { get; protected set; }
    }
    
    public interface ICharacter : IUnit, ICanUseSkill, ICanDef
    {
        // 角色自己的一些方法
        void Move(UnityEngine.Vector3 destination);
    }
}
