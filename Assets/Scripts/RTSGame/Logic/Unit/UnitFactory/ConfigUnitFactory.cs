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
}
