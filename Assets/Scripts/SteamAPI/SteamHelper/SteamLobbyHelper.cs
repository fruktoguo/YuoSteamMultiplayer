using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

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
                LobbyMatchList_t result = await SteamMatchmakingHelper.RequestLobbyList().Await<LobbyMatchList_t>();

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
    }
}