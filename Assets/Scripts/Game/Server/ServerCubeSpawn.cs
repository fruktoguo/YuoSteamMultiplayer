using System;
using FishNet.Object;
using UnityEngine;

namespace Game.Server
{ 
    public class ServerCubeSpawnManager : NetworkBehaviour
    {
        public GameObject cubePrefab;
        private float _spawnTime;
        private float _beforeSpawnTime;

        private void Awake()
        {
            _spawnTime = 1;
            _beforeSpawnTime = 0;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnCube()
        { 
            var tmpCube = Instantiate(cubePrefab); 
            tmpCube.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            ServerManager.Spawn(tmpCube);
        }

        private void Update()
        { 
            if (!IsServerInitialized)
            {
                Debug.Log("Server not initialized"); 
                return;
            }
             
            // TimeManager.ServerUptime 
            // 写个倒计时，每隔一秒生成一个方块，位置随机
            if (TimeManager.ServerUptime - _beforeSpawnTime > _spawnTime)
            {
                _beforeSpawnTime = TimeManager.ServerUptime;
                SpawnCube();
            }
        } 
    }
}
