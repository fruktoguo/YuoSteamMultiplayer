using FishNet.Object;
using UnityEngine.Events;
using YuoTools;

namespace Game
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance;
        public DataManager GlobalData;

        public UnityEvent OnGameStart;

        public void Awake()
        {
            Instance = this;
            GlobalData = new DataManager();
            OnGameStart.AddListener(() => "开始游戏".Log());
        }

        public void StartGame()
        {
            if (IsServerInitialized)
            {
                "服务端开始游戏".Log();
                RpcGameStart();
            }
        }

        [ObserversRpc]
        void RpcGameStart()
        {
            OnGameStart?.Invoke();
        }


        public void CreatePlayerData(string id)
        {
        }
    }
}