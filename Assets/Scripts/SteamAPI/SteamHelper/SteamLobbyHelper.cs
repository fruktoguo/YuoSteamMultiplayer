using System.Threading.Tasks;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SteamAPI.SteamHelper
{
    public static class SteamLobbyHelper
    {
        // 异步请求大厅列表，返回符合条件的大厅 ID 数组
        public static async Task<Lobby[]> FindLobbyListAsync()
        {
            try
            {
                // 使用Await方法等待大厅列表请求结果
                LobbyMatchList_t result = await SteamMatchmakingHelper.RequestLobbyList().WaitAsync<LobbyMatchList_t>();

                int lobbyCount = (int)result.m_nLobbiesMatching;
                Lobby[] lobbyIds = new Lobby[lobbyCount];

                for (int i = 0; i < lobbyCount; i++)
                {
                    lobbyIds[i] = SteamMatchmakingHelper.GetLobbyByIndex(i);
                }

                Debug.Log($"找到 {lobbyCount} 个大厅");
                return lobbyIds;
            }
            catch (System.Exception ex)
            {
                HandleError($"请求大厅列表时发生错误: {ex.Message}");
                return null;
            }
        }

        // 统一处理错误信息
        private static void HandleError(string message)
        {
            Debug.LogError(message);
        }


        public static void SetLobbyFindFilter()
        {
            SteamMatchmakingHelper.AddRequestLobbyListStringFilter(SteamHelper.GameFilterKey, SteamHelper.GameFilterKey,
                ELobbyComparison.k_ELobbyComparisonEqual);
        }

        /// <summary>
        /// 创建Steam大厅 
        /// </summary>
        public static async Task<Lobby?> CreateLobby(ELobbyType eLobbyType = ELobbyType.k_ELobbyTypePublic,
            int maxPlayers = 4)
        {
            LobbyCreated_t lobbyCreatedT =
                await SteamMatchmaking.CreateLobby(eLobbyType, maxPlayers).WaitAsync<LobbyCreated_t>();
            if (lobbyCreatedT.m_eResult != EResult.k_EResultOK)
            {
                Debug.Log("大厅创建失败");
                return null;
            }

            string userId = SteamAPIManager.LocalUserSteamID.ToString();
            var lobbyId = new CSteamID(lobbyCreatedT.m_ulSteamIDLobby);
            Lobby lobby = lobbyId;
            // 创建大厅成功后，设置大厅数据
            // 存储房主的SteamID，用于后续的服务器创建
            lobby.SetData(SteamHelper.HouseOwnerKey, userId); // 房主的SteamID
            lobby.SetData(nameof(SteamHelper.GameFilterKey), SteamHelper.GameFilterKey);
            Debug.Log("serverUserId  大厅创建成功:" + userId);
            SteamAPIManager.Instance.fishySteamworks.SetClientAddress(userId); // 设置房主的客户端地址

            SteamAPIManager.Instance.fishySteamworks.StartConnection(true);
            SteamAPIManager.Instance.fishySteamworks.StartConnection(false);
            return lobby;
        }

        /// <summary>
        /// 加入Steam大厅 
        /// </summary>
        /// <param name="lobbyId"> 大厅id </param>
        public static async Task<LobbyEnter_t> JoinLobby(CSteamID lobbyId)
        {
            var lobbyEnter = await SteamMatchmaking.JoinLobby(lobbyId).WaitAsync<LobbyEnter_t>();

            if (lobbyEnter.m_EChatRoomEnterResponse != (int)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
            {
                Debug.Log("加入大厅失败");
                return lobbyEnter;
            }

            Lobby lobby = lobbyId;
            // 处理成功加入大厅的其他逻辑
            var serverUserId = lobby.GetData(SteamHelper.HouseOwnerKey);

            Debug.Log("serverUserId  加入大厅:" + serverUserId);

            SteamAPIManager.Instance.fishySteamworks.SetClientAddress(serverUserId); // 设置房主的客户端地址

            SteamAPIManager.Instance.fishySteamworks.StartConnection(false); // 链接客户端
            return lobbyEnter;
        }
    }
}