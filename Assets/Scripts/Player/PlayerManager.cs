using Unity.Netcode;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private void OnClientDisconnectCallback(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (player != null)
            {
                Destroy(player.gameObject);
            }
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    // private void OnDestroy()
    // {
    //     NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    // }
}