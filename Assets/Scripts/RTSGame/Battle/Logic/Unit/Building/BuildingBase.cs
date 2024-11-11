namespace RTSGame
{
    
    public abstract class BuildingData : UnitData, IBuildingData
    {
        public BuildingType BuildingType { get; protected set; }
        protected BuildingData(UnitType unitType,BuildingType buildingType, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue) : base(unitType, name, maxHp, curHp, atkNum, cost, buildTime, hpRecoverySpeed, hpRecoveryValue)
        {
            BuildingType = buildingType;
        }
        
    } 
    
    public abstract class BuildingBase : UnitBase , IBuilding
    {
        protected BuildingBase(BuildingData data) : base(data)
        {
        }
    }
    
}