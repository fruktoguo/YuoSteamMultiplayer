using Steamworks;
using UnityEngine;

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
            SteamAPIManager.Instance.fishySteamworks.StartConnection(false);   // 链接客户端
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
    
    
    /// <summary>
    /// 创建Steam大厅 
    /// </summary>
    public static void CreateLobby(ELobbyType eLobbyType = ELobbyType.k_ELobbyTypePublic,int maxPlayers = 4)
    {
        CallResult<LobbyCreated_t> lobbyCreated = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
        var handle = SteamMatchmaking.CreateLobby(eLobbyType,maxPlayers);
        lobbyCreated.Set(handle);
    }
    
    /// <summary>
    /// 创建Steam大厅回调
    /// </summary> 
    private static void OnLobbyCreated(LobbyCreated_t lobby, bool bIOFailure)
    {
        if (lobby.m_eResult != EResult.k_EResultOK || bIOFailure)
        {
            Debug.Log("大厅创建失败");
            return;
        }
                 
        string userId = SteamAPIManager.LocalUserSteamID.ToString();
        // 创建大厅成功后，设置大厅数据
        // 存储房主的SteamID，用于后续的服务器创建
        SetLobbyData(new CSteamID(lobby.m_ulSteamIDLobby), "HouseOwnerServerPost", userId);  // 房主的SteamID
        Debug.Log("serverUserId  大厅创建成功:" + userId);
        // todo：这里可以设置其他的大厅数据，如房间名等
        SteamAPIManager.Instance.fishySteamworks.SetClientAddress(userId); // 设置房主的客户端地址
        SteamAPIManager.Instance.fishySteamworks.StartConnection(true);  // 设置当前的主机为自己
    }
    
    /// <summary>
    /// 加入Steam大厅 
    /// </summary>
    /// <param name="lobbyId"> 大厅id </param>
    public static void JoinLobby(ulong lobbyId)
    {
        CallResult<LobbyEnter_t> lobbyEnter = CallResult<LobbyEnter_t>.Create(OnLobbyEnter);
        CSteamID csSteamLobbyId = new CSteamID(lobbyId); 
        var handle = SteamMatchmaking.JoinLobby(csSteamLobbyId);
        lobbyEnter.Set(handle);
    }
    
    /// <summary>
    /// 其他玩家链接加入Steam大厅回调 
    /// </summary> 
    private static void OnLobbyEnter(LobbyEnter_t lobbyEnter, bool bIOFailure) 
    {
        if (lobbyEnter.m_EChatRoomEnterResponse != (int)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess || bIOFailure)
        {
            Debug.Log("加入大厅失败");
            return;
        } 
        // 处理成功加入大厅的其他逻辑
        var serverUserId = GetLobbyData(new CSteamID(lobbyEnter.m_ulSteamIDLobby), "HouseOwnerServerPost");
        Debug.Log("serverUserId  加入大厅:" + serverUserId);
        SteamAPIManager.Instance.fishySteamworks.SetClientAddress(serverUserId); // 设置房主的客户端地址
        SteamAPIManager.Instance.fishySteamworks.StartConnection(false);   // 链接客户端
    }
    
    /// <summary>
    /// 获取steam大厅列表
    /// </summary>
    public static void GetLobbyList()
    {
        CallResult<LobbyMatchList_t> lobbyMatchList = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
        // SteamMatchmaking.AddRequestLobbyListStringFilter("HUSEJIN","HUSEJIN",ELobbyComparison.k_ELobbyComparisonEqual);  // 过滤
        var handle = SteamMatchmaking.RequestLobbyList();
        lobbyMatchList.Set(handle);
    }
    
    private static void OnLobbyMatchList(LobbyMatchList_t lobby, bool bIOFailure)
    {
        if (lobby.m_nLobbiesMatching == 0 || bIOFailure)
        {
            Debug.Log("获取大厅列表失败");
            return;
        }

        for (int i = 0; i < lobby.m_nLobbiesMatching; i++)
        {
            var lobbyCSteamID = SteamMatchmaking.GetLobbyByIndex(i); 
            
            var count = SteamMatchmaking.GetLobbyDataCount(lobbyCSteamID);
            Debug.Log("id:"+lobbyCSteamID+" ---- count:"+count);
        }
                
        Debug.Log("获取大厅列表成功,大厅数量:" + lobby.m_nLobbiesMatching);
    }
    
    /// <summary>
    /// 离开大厅 
    /// </summary> 
    public static void LeaveLobby(ulong lobbyId)
    {
        CSteamID csSteamLobbyId = new CSteamID(lobbyId); 
        SteamMatchmaking.LeaveLobby(csSteamLobbyId);
    }
    
    private void OnLeaveLobby(LobbyChatUpdate_t lobbyChatUpdate)
    {
        Debug.Log("离开大厅成功");
    }
    
    public static void SetLobbyData(CSteamID lobbyId, string key, string value)
    { 
        SteamMatchmaking.SetLobbyData(lobbyId, key, value);
    }
    
    public static string GetLobbyData(CSteamID lobbyId, string key="")
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
