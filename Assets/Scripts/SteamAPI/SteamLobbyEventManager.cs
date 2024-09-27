using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SteamAPI.SteamHelper;
using Steamworks;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using YuoTools;

public class OnLobbyDataUpdateEvent : IEventMessage
{
    public LobbyDataUpdate_t lobbyDataUpdate;
}


public class SteamLobbyEventManager : MonoBehaviour
{
    #region 事件回调

    Callback<LobbyChatUpdate_t> onLobbyChatUpdate;
    Callback<LobbyChatMsg_t> onLobbyChatMsg;
    Callback<LobbyDataUpdate_t> onLobbyDataUpdate;
    Callback<LobbyEnter_t> onLobbyEnter;
    Callback<LobbyGameCreated_t> onLobbyGameCreated;
    Callback<LobbyKicked_t> onLobbyKicked;
    Callback<LobbyCreated_t> onLobbyCreated;

    public void RegisterEvents()
    {
        onLobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
        onLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        onLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        onLobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        onLobbyGameCreated = Callback<LobbyGameCreated_t>.Create(OnLobbyGameCreated);
        onLobbyKicked = Callback<LobbyKicked_t>.Create(OnLobbyKicked);
        onLobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
    }

    public void UnRegisterEvents()
    {
        onLobbyChatUpdate?.Dispose();
        onLobbyChatMsg?.Dispose();
        onLobbyDataUpdate?.Dispose();
        onLobbyEnter?.Dispose();
        onLobbyGameCreated?.Dispose();
        onLobbyKicked?.Dispose();
        onLobbyCreated?.Dispose();
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t lobbyChatUpdate)
    {
        Debug.Log("大厅聊天更新");
    }

    private void OnLobbyChatMsg(LobbyChatMsg_t lobbyChatMsg)
    {
        Debug.Log("大厅聊天消息");
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t lobbyDataUpdate)
    { 
        SteamAPIManager.Instance.OnLobbyDataUpdate(lobbyDataUpdate); 
        Debug.Log("大厅数据更新");
    }

    private void OnLobbyEnter(LobbyEnter_t lobbyEnter)
    {
        if (lobbyEnter.m_EChatRoomEnterResponse == (int)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            var lobbyId = new CSteamID(lobbyEnter.m_ulSteamIDLobby);
            var lobbyOwner = GetLobbyData(lobbyId, SteamHelper.HouseOwnerKey);
            var lobbyOwnerId = new CSteamID(Convert.ToUInt64(lobbyOwner));
            Lobby lobby = lobbyId;
            SteamAPIManager.Instance.CurrentLobby = lobby;
            Debug.Log(
                $"加入 玩家 {SteamFriendsHelper.GetFriendPersonaName(lobbyOwnerId)}({lobbyOwner}) 的房间 {lobby.LobbyName}");
        }
    }

    private void OnLobbyGameCreated(LobbyGameCreated_t lobbyGameCreated)
    {
        Debug.Log("大厅游戏创建");
    }

    private void OnLobbyKicked(LobbyKicked_t lobbyKicked)
    {
        Debug.Log("大厅踢出");
    }

    private void OnLobbyCreated(LobbyCreated_t lobbyCreated)
    {
        Debug.Log("大厅创建");
    }

    #endregion


    private void Awake()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnRegisterEvents();
    }

    private void OnLeaveLobby(LobbyChatUpdate_t lobbyChatUpdate)
    {
        Debug.Log("离开大厅成功");
        SteamAPIManager.Instance.CurrentLobby = null;
    }

    public void SetLobbyData(CSteamID lobbyId, string key, string value)
    {
        SteamMatchmaking.SetLobbyData(lobbyId, key, value);
    }

    public string GetLobbyData(CSteamID lobbyId, string key = "")
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
    public void SetLobbyGameServer(CSteamID lobbyId, uint serverIp, ushort serverPort, CSteamID steamId)
    {
        SteamMatchmaking.SetLobbyGameServer(lobbyId, serverIp, serverPort, steamId);
    }

    public void SetLobbyMemberData(CSteamID lobbyId, string key, string value)
    {
        SteamMatchmaking.SetLobbyMemberData(lobbyId, key, value);
    }
}