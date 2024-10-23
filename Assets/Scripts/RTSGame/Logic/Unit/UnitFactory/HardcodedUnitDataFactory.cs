using System;

namespace RTSGame
{
    public class HardcodedUnitDataFactory : IUnitDataFactory
    {
        public BuildingData CreateBuildingData(BuildingType buildingType)
        {
            switch (buildingType)
            {
                case BuildingType.Farm:    // TODO
                    // return new BuildingData
                    // {
                    //     UnitType = UnitType.Building,
                    //     Name = "Farm",
                    //     MaxHp = 1000,
                    //     Cost = 100,
                    //     BuildTime = 30
                    // };
                case BuildingType.Barracks:
                    // return new BuildingData
                    // {
                    //     UnitType = UnitType.Building,
                    //     Name = "Barracks",
                    //     MaxHp = 1500,
                    //     Cost = 200,
                    //     BuildTime = 45
                    // };
                default:
                    throw new ArgumentException("Unknown building type");
            }
        }

        public CharacterData CreateCharacterData(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Soldier:
                    // return new CharacterData
                    // {
                    //     UnitType = UnitType.Character,
                    //     CharacterType = CharacterType.Soldier,
                    //     Name = "Soldier",
                    //     MaxHp = 100,
                    //     AtkNum = 10,
                    //     MovementSpeed = 5f
                    // };
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