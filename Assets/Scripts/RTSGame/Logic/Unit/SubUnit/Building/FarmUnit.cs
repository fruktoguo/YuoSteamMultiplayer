namespace RTSGame
{
    public class FarmBuildingData : BuildingData
    {
        public FarmBuildingData(UnitType unitType, BuildingType buildingType, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue) : base(unitType, buildingType, name, maxHp, curHp, atkNum, cost, buildTime, hpRecoverySpeed, hpRecoveryValue)
        {
        }
    }
    
    /// <summary>
    /// 农场
    /// </summary>
    public class FarmUnit : BuildingBase
    {
        public FarmUnit(BuildingData data) : base(data)
        {
        }
    }
}
