namespace RTSGame
{
    public interface IBuildingData : IUnitData
    { 
        // 建筑自己的数据, 之后都走配置表
        public BuildingType BuildingType { get; }
    }
    
    public interface IBuilding : IUnit
    {
        // 建筑自己的一些方法
    }
}
