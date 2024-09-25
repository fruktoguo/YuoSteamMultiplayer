using System.Threading.Tasks;
using SteamAPI.SteamHelper;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamLobbyManager : MonoBehaviour
{
    #region 事件回调

    public static void RegisterEvents()
    {
        Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
        Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        Callback<LobbyGameCreated_t>.Create(OnLobbyGameCreated);
        Callback<LobbyKicked_t>.Create(OnLobbyKicked);
        Callback<LobbyCreated_t>.Create(OnLobbyCreated);
    }

    private static void OnLobbyChatUpdate(LobbyChatUpdate_t lobbyChatUpdate)
    {
        Debug.Log("大厅聊天更新");
    }

    private static void OnLobbyChatMsg(LobbyChatMsg_t lobbyChatMsg)
    {
        Debug.Log("大厅聊天消息");
    }

    private static void OnLobbyDataUpdate(LobbyDataUpdate_t lobbyDataUpdate)
    {
        Debug.Log("大厅数据更新");
    }

    private static void OnLobbyEnter(LobbyEnter_t lobbyEnter)
    {
        if (lobbyEnter.m_EChatRoomEnterResponse == (int)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            var serverUserId = GetLobbyData(new CSteamID(lobbyEnter.m_ulSteamIDLobby), "HouseOwnerServerPost");
            Debug.Log("serverUserId  加入大厅:" + serverUserId);
            SteamAPIManager.Instance.fishySteamworks.SetClientAddress(serverUserId); // 设置房主的客户端地址
            SteamAPIManager.Instance.fishySteamworks.StartConnection(false); // 链接客户端
            
            SceneManager.LoadScene("GameScene");
            SteamAPIManager.Instance.networkManager.ClientManager.StartConnection(); 
        }
    }

    private static void OnLobbyGameCreated(LobbyGameCreated_t lobbyGameCreated)
    {
        Debug.Log("大厅游戏创建");
    }

    private static void OnLobbyKicked(LobbyKicked_t lobbyKicked)
    {
        Debug.Log("大厅踢出");
    }

    private static void OnLobbyCreated(LobbyCreated_t lobbyCreated)
    {
        Debug.Log("大厅创建");
    }

    #endregion

    private void OnLeaveLobby(LobbyChatUpdate_t lobbyChatUpdate)
    {
        Debug.Log("离开大厅成功");
    }

    public static void SetLobbyData(CSteamID lobbyId, string key, string value)
    {
        SteamMatchmaking.SetLobbyData(lobbyId, key, value);
    }

    public static string GetLobbyData(CSteamID lobbyId, string key = "")
    {
        return SteamMatchmaking.GetLobbyData(lobbyId, key);
    }

    /// <summary>
    /// 设置大厅所关联的游戏服务器的信息
    /// </summary>
    /// <param name="lobbyId">大厅ID</param>
    /// <param name="serverIp">服务器IP</param>
    /// <param name="serverPort">服务器端口</param>
    /// <param name="steamId">服务器Steam ID</param>
    public static void SetLobbyGameServer(CSteamID lobbyId, uint serverIp, ushort serverPort, CSteamID steamId)
    {
        SteamMatchmaking.SetLobbyGameServer(lobbyId, serverIp, serverPort, steamId);
    }

    public static void SetLobbyMemberData(CSteamID lobbyId, string key, string value)
    {
        SteamMatchmaking.SetLobbyMemberData(lobbyId, key, value);
    }
}