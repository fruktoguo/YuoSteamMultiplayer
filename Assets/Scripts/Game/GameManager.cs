using System;
using FishNet.Object;
using Game.Event;
using UniFramework.Event;
using UnityEngine.Events;
using YuoTools;

namespace Game
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance;
        public DataManager GlobalData; 

        public void Awake()
        {
            Instance = this;
            GlobalData = new DataManager();
            UniEvent.Initalize();
            AddListeners();
        }
        
        private void AddListeners()
        {
            UniEvent.AddListener<S2C_StartEvent>(SendStartGameLog);
        }

        private void RemoveListeners()
        {
            UniEvent.RemoveListener<S2C_StartEvent>(SendStartGameLog); 
        }

        private void SendStartGameLog(IEventMessage startEvent)
        {
            "开始游戏".Log();
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
            UniEvent.SendMessage(new S2C_StartEvent());
        }


        public void CreatePlayerData(string id)
        {
        }

        private void OnDestroy()
        {
            RemoveListeners();
            UniEvent.Destroy();
        }
    }
}