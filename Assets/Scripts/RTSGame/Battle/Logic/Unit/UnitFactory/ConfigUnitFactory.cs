using System;

namespace RTSGame
{
    /// <summary>
    /// 使用配置的方式创建单位   TODO: 
    /// </summary>
    public class ConfigUnitFactory : IUnitFactory
    {
        private string _todoConfig; // 占位

        public ConfigUnitFactory(string todoConfig)
        {
            _todoConfig = todoConfig; 
        }

        public IUnitDataFactory DataFactory { get; }
        
        public ConfigUnitFactory()
        {
            DataFactory = new HardcodedUnitDataFactory();
        }

        public IBuilding CreateBuilding(BuildingType buildingType)
        {
            throw new System.NotImplementedException();
        }

        public ICharacter CreateCharacter(CharacterType characterType)
        {
            throw new System.NotImplementedException();
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
