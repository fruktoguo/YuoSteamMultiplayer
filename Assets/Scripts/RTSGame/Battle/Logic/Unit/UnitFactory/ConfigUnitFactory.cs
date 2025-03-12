using System;

namespace RTSGame
{
    /// <summary>
    /// 使用配置的方式创建单位  
    /// </summary>
    public class ConfigUnitFactory : IUnitFactory
    {
        private string _todoConfig; // 占位

        public ConfigUnitFactory(string todoConfig)
        {
            _todoConfig = todoConfig; 
        }
 
        
        public ConfigUnitFactory()
        { 
        } 
        
        public IUnit CreateUnit(int unitID)
        {
            throw new System.NotImplementedException();
        }
    }
}
