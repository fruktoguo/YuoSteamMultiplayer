using System.Collections.Generic;
using Sirenix.OdinInspector;
using Steamworks;
using YuoTools;

namespace Game.Manager
{
    public class PlayerManager : SingletonMono<PlayerManager>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public PlayerClientManager LocalPlayer;

        [ShowInInspector] public HashSet<PlayerClientManager> allPlayers = new();

        [ShowInInspector] public Dictionary<CSteamID, PlayerClientManager> playerMapFromSteamId = new();
        [ShowInInspector] public Dictionary<int, PlayerClientManager> playerMapFromNetworkId = new();

        public static void RegisterPlayer(PlayerClientManager player)
        {
            Instance.allPlayers.Add(player);
            Instance.playerMapFromNetworkId[player.OwnerId] = player;
            Instance.playerMapFromSteamId[player.steamID] = player;

            if (player.IsOwner)
            {
                Instance.LocalPlayer = player;
            }
        }

        public static void UnRegisterPlayer(PlayerClientManager player)
        {
            Instance.allPlayers.Remove(player);
            Instance.playerMapFromNetworkId.Remove(player.OwnerId);
            Instance.playerMapFromSteamId.Remove(player.steamID);

            if (Instance.LocalPlayer == player)
            {
                Instance.LocalPlayer = null;
            }
        }
    }
}