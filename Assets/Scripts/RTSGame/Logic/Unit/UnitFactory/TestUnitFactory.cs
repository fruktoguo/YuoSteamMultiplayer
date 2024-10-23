using System;

namespace RTSGame
{
    /// <summary>
    /// 测试用的创建
    /// </summary>
    public class TestUnitFactory : IUnitFactory
    {  
        public IUnitDataFactory DataFactory { get; }
        public TestUnitFactory()
        {
            DataFactory = new HardcodedUnitDataFactory();
        }
        
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
}
