using YuoTools;

namespace Game
{
    public class GameManager : SingletonMono<GameManager>
    {
        public DataManager GlobalData;

        public override void Awake()
        {
            base.Awake();
            GlobalData = new DataManager();
        }


        public void CreatePlayerData(string id)
        {
            
            
        }
        
        
        
        
        
    }
}