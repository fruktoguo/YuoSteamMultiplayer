namespace RTSGame
{
    
    public class BarracksBuildingData : BuildingData
    {
        public BarracksBuildingData(UnitType unitType, BuildingType buildingType, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue) : base(unitType, buildingType, name, maxHp, curHp, atkNum, cost, buildTime, hpRecoverySpeed, hpRecoveryValue)
        {
        }
    }

    
    /// <summary>
    /// 兵营
    /// </summary>
    public class BarracksUnit : BuildingBase
    {
        public BarracksUnit(BuildingData data) : base(data)
        {
        }
    }
}
