using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SteamLobbyManager : SerializedMonoBehaviour
{
    private CallResult<LobbyCreated_t> createLobbyCallResult;
    private CallResult<LobbyMatchList_t> findAllLobbiesCallResult;
    private CallResult<LobbyEnter_t> joinLobbyCallResult;

    private const string LOBBY_KEY = "unique_key";
    private const string LOBBY_VALUE = "my_unique_value";

    public event Action<string> OnLobbyListUpdated;
    public event Action<string> OnChatRoomUpdated;
    public event Action<bool, string> OnLobbyCreatedCallback;
    public event Action<bool, string> OnLobbyListReceivedCallback;
    public event Action<bool, string> OnLobbyEnteredCallback;
    public event Action OnLobbyExitedCallback; // 新增事件

    public List<CSteamID> lobbyList = new();
    public List<CSteamID> playerList = new();

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            throw new Exception("Steam SDK未初始化！");
        }

        createLobbyCallResult = new CallResult<LobbyCreated_t>(OnLobbyCreated);
        findAllLobbiesCallResult = new CallResult<LobbyMatchList_t>(OnLobbyListReceived);
        joinLobbyCallResult = new CallResult<LobbyEnter_t>(OnLobbyEntered);
    }

    public void CreateLobby()
    {
        SteamAPICall_t createLobbyCallback = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
        createLobbyCallResult.Set(createLobbyCallback);
        Debug.Log("创建大厅...");
    }

    private void OnLobbyCreated(LobbyCreated_t result, bool bIOFailure)
    {
        if (bIOFailure || result.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("创建大厅失败！");
            OnLobbyCreatedCallback?.Invoke(false, "创建大厅失败！");
            return;
        }

        Debug.Log($"创建大厅成功！ID: {result.m_ulSteamIDLobby}");
        OnLobbyCreatedCallback?.Invoke(true, $"创建大厅成功！ID: {result.m_ulSteamIDLobby}");

        // 设置自定义数据
        CSteamID lobbyID = new CSteamID(result.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(lobbyID, LOBBY_KEY, LOBBY_VALUE);

        // 直接加入创建的大厅
        JoinLobby(new CSteamID(result.m_ulSteamIDLobby));
    }

    public void FindAllLobbies()
    {
        // 使用自定义数据进行过滤
        SteamMatchmaking.AddRequestLobbyListStringFilter(LOBBY_KEY, LOBBY_VALUE,
            ELobbyComparison.k_ELobbyComparisonEqual);
        SteamAPICall_t findAllLobbiesCallback = SteamMatchmaking.RequestLobbyList();
        findAllLobbiesCallResult.Set(findAllLobbiesCallback);
        Debug.Log("请求查找所有大厅...");
    }

    private void OnLobbyListReceived(LobbyMatchList_t result, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.Log("获取大厅列表失败！");
            OnLobbyListReceivedCallback?.Invoke(false, "获取大厅列表失败！");
            return;
        }

        Debug.Log($"找到的大厅数量: {result.m_nLobbiesMatching}");
        OnLobbyListReceivedCallback?.Invoke(true, $"找到的大厅数量: {result.m_nLobbiesMatching}");

        lobbyList.Clear();
        string lobbyListString = "";
        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyList.Add(lobbyID);
            lobbyListString += $"大厅ID: {lobbyID}\n";
        }

        OnLobbyListUpdated?.Invoke(lobbyListString);
    }

    public void JoinLobby(CSteamID lobbyID)
    {
        SteamAPICall_t joinLobbyCallback = SteamMatchmaking.JoinLobby(lobbyID);
        joinLobbyCallResult.Set(joinLobbyCallback);
        // Debug.Log($"尝试加入大厅: {lobbyID}");
    }

    public CSteamID NowLobbyID;

    private void OnLobbyEntered(LobbyEnter_t result, bool bIOFailure)
    {
        if (bIOFailure || result.m_EChatRoomEnterResponse !=
            (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            Debug.Log("加入大厅失败！");
            OnLobbyEnteredCallback?.Invoke(false, "加入大厅失败！");
            return;
        }

        NowLobbyID = new CSteamID(result.m_ulSteamIDLobby);
        int memberCount = SteamMatchmaking.GetNumLobbyMembers(NowLobbyID);
        Debug.Log($"加入大厅成功！大厅ID: {NowLobbyID}，成员数量: {memberCount}");
        OnLobbyEnteredCallback?.Invoke(true, $"加入大厅成功！大厅ID: {NowLobbyID}，成员数量: {memberCount}");

        playerList.Clear();

        string chatRoom = "";
        for (int i = 0; i < memberCount; i++)
        {
            CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(NowLobbyID, i);
            string memberName = SteamFriends.GetFriendPersonaName(memberID);
            chatRoom += $"ID: {memberID} 名称: {memberName} 已加入房间\n";
            playerList.Add(memberID);
        }

        OnChatRoomUpdated?.Invoke(chatRoom);

        // 启动处理P2P消息的协程
        coroutineMessage = StartCoroutine(HandleP2PPackets());

        // 启动监听成员变化的协程
        coroutineMember = StartCoroutine(WatchLobbyMembers());
    }

    private IEnumerator WatchLobbyMembers()
    {
        while (true)
        {
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(NowLobbyID);
            for (int i = 0; i < memberCount; i++)
            {
                CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(NowLobbyID, i);
                if (!playerList.Contains(memberID))
                {
                    // 新成员加入
                    string memberName = SteamFriends.GetFriendPersonaName(memberID);
                    Debug.Log($"ID: {memberID} 名称: {memberName} 已加入房间");
                    OnChatRoomUpdated?.Invoke($"ID: {memberID} 名称: {memberName} 已加入房间");
                    playerList.Add(memberID);
                }
            }

            yield return new WaitForSeconds(1f); // 每秒检查一次
        }
    }

    private Coroutine coroutineMessage;
    private Coroutine coroutineMember;

    public void ExitLobby()
    {
        // 调用Steam内置方法退出大厅
        SteamMatchmaking.LeaveLobby(NowLobbyID);
        NowLobbyID = CSteamID.Nil;
        // 退出大厅的逻辑
        Debug.Log("退出大厅");
        OnLobbyExitedCallback?.Invoke();
        if (coroutineMessage != null) StopCoroutine(coroutineMessage);
        if (coroutineMember != null) StopCoroutine(coroutineMember);
    }

    public void SendP2PRequest(CSteamID memberID, string message)
    {
        string memberName = SteamFriends.GetFriendPersonaName(memberID);
        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);

        // 添加日志，检查消息大小
        Debug.Log($"尝试发送P2P消息给 {memberName} (ID: {memberID})，消息大小: {messageBytes.Length} 字节");

        if (SteamNetworking.SendP2PPacket(memberID, messageBytes, (uint)messageBytes.Length,
                EP2PSend.k_EP2PSendReliable))
        {
            Debug.Log($"已发送P2P消息给 {memberName} (ID: {memberID})");
        }
        else
        {
            Debug.Log($"无法发送P2P消息给 {memberName} (ID: {memberID})");
        }
    }

    public void TransferLobbyOwnership(CSteamID newOwnerID)
    {
        // 检查当前玩家是否为房主
        CSteamID currentOwnerID = SteamMatchmaking.GetLobbyOwner(NowLobbyID);
        if (currentOwnerID != SteamUser.GetSteamID())
        {
            Debug.Log("只有房主可以转交房主权限！");
            return;
        }

        // 转交房主
        SteamMatchmaking.SetLobbyOwner(NowLobbyID, newOwnerID);
        Debug.Log($"房主已转交给: {newOwnerID}");
    }

    private void Update()
    {
        SteamAPI.RunCallbacks();
    }

    public IEnumerator HandleP2PPackets()
    {
        while (true)
        {
            while (SteamNetworking.IsP2PPacketAvailable(out var packetSize))
            {
                byte[] buffer = new byte[packetSize];
                CSteamID remoteId;
                uint bytesRead;
                if (SteamNetworking.ReadP2PPacket(buffer, packetSize, out bytesRead, out remoteId))
                {
                    string message = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)bytesRead);
                    string memberName = SteamFriends.GetFriendPersonaName(remoteId);
                    Debug.Log($"收到来自 {memberName} (ID: {remoteId}) 的P2P消息: {message}");

                    // 更新聊天房间
                    OnChatRoomUpdated?.Invoke($"{memberName}: {message}");
                }
            }

            yield return null; // 等待下一帧
        }
    }

    public List<CSteamID> GetLobbyList()
    {
        return new List<CSteamID>(lobbyList);
    }

    public List<CSteamID> GetPlayerList()
    {
        return new List<CSteamID>(playerList);
    }
}