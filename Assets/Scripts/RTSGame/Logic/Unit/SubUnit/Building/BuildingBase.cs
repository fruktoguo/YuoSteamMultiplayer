namespace RTSGame
{
    
    public abstract class BuildingData : UnitData, IBuildingData
    {
        public BuildingType BuildingType { get; protected set; }
    } 
    
    public abstract class BuildingBase : UnitBase , IBuilding
    {
        protected BuildingBase(BuildingData data) : base(data)
        {
        }
    }
    
}