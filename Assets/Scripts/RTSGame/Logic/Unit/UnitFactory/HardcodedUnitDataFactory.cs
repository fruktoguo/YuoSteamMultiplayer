using System;

namespace RTSGame
{
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

    public class ConfigUnitDataFactory : IUnitDataFactory
    {
        private string _configPath;

        public ConfigUnitDataFactory(string configPath)
        {
            _configPath = configPath;
        }

        public BuildingData CreateBuildingData(BuildingType buildingType)
        {
            // 从配置文件加载数据
            // 这里只是示例，实际实现需要根据您的配置文件格式来处理
            // return LoadBuildingDataFromConfig(_configPath, buildingType);
            throw new NotImplementedException();
        }

        public CharacterData CreateCharacterData(CharacterType characterType)
        {
            // 从配置文件加载数据
            // return LoadCharacterDataFromConfig(_configPath, characterType);
            throw new NotImplementedException();
        }
    }

}