// using System.Collections.Generic;
// using FishNet.Object;
// using UnityEngine;
//
// namespace Game.Server
// {
//     public class ServerSpawnManager : NetworkBehaviour
//     {
//         private Dictionary<string,GameObject> _spawnedObjects = new();
//         
//         public void Spawn(string prefabName)
//         {
//             if (_spawnedObjects.ContainsKey(prefabName))
//             {
//                 return;
//             }
//             
//             ServerManager.Spawn(_spawnedObjects[prefabName]);
//         }
//     }
// }
