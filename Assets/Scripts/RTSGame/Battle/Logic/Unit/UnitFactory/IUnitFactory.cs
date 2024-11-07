namespace RTSGame
{
    public interface IUnitFactory
    { 
        IUnitDataFactory DataFactory { get; }
        IBuilding CreateBuilding(BuildingType buildingType);
        ICharacter CreateCharacter(CharacterType characterType); 
    }
    public interface IUnitDataFactory
    {
        BuildingData CreateBuildingData(BuildingType buildingType);
        CharacterData CreateCharacterData(CharacterType characterType);
    }
}
