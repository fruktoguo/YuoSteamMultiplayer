using System;

namespace RTSGame
{
    /// <summary>
    /// 测试用的创建
    /// </summary>
    public class TestUnitFactory : IUnitFactory
    {  
        public IUnitDataFactory DataFactory { get; } = new HardcodedUnitDataFactory();

        public IBuilding CreateBuilding(BuildingType buildingType)
        {
            BuildingData buildingData = DataFactory.CreateBuildingData(buildingType);
        
            switch (buildingType)
            {
                case BuildingType.Farm:
                    return new FarmUnit(buildingData);
                case BuildingType.Barracks:
                    return new BarracksUnit(buildingData);
                default:
                    throw new ArgumentException("Unknown building type");
            }
        }
        
        
        public ICharacter CreateCharacter(CharacterType characterType)
        {
            CharacterData characterData = DataFactory.CreateCharacterData(characterType);
        
            switch (characterType)
            {
                case CharacterType.Soldier:
                    return new SoldierUnit(characterData);
                case CharacterType.Worker:
                    return new WorkerUnit(characterData);
                default:
                    throw new ArgumentException("Unknown character type");
            }
        }
    }
    
    
    public class HardcodedUnitDataFactory : IUnitDataFactory
    {
        public BuildingData CreateBuildingData(BuildingType buildingType)
        {
            switch (buildingType)
            {
                case BuildingType.Farm:   
                    return new FarmBuildingData(UnitType.Building, BuildingType.Farm, "Farm", 1000, 1000, 0, 100, 30, 1, 10);
                case BuildingType.Barracks:
                    return new BarracksBuildingData(UnitType.Building, BuildingType.Barracks, "Barracks", 1500, 1500, 0, 200, 45, 1, 10);
                default:
                    throw new ArgumentException("Unknown building type");
            }
        }

        public CharacterData CreateCharacterData(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Soldier:
                    return new SoldierCharacterData(UnitType.Character, CharacterType.Soldier, 1.0f,"Soldier", 100, 100, 10, 50, 10, 1, 10);
                case CharacterType.Worker:
                    return new WorkerCharacterData(UnitType.Character, CharacterType.Worker, 1.0f,"Worker", 100, 100, 5, 30, 10, 1, 10);
                default:
                    throw new ArgumentException("Unknown character type");
            }
        }
    }
}
