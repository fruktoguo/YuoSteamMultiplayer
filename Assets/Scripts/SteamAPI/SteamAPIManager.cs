using System;
using System.Collections.Generic;
using FishNet.Managing;
using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using SteamAPI.SteamHelper;
using Steamworks;
using UniFramework.Event;
using UnityEngine;
using YuoTools;

public class SteamAPIManager : SteamworksBehaviour
{
    public static ulong LocalUserSteamID;

    public Lobby? CurrentLobby;

    public NetworkManager networkManager;
    public FishySteamworks.FishySteamworks fishySteamworks;

    private void Init()
    {
        // 初始化steamWork
        if (!Steamworks.SteamAPI.Init())
        {
            Debug.LogError("SteamAPI.Init() failed.");
            return;
        }  
        DontDestroyOnLoad(gameObject);
        LocalUserSteamID = SteamUser.GetSteamID().m_SteamID;
    }

    public void OnLobbyDataUpdate(LobbyDataUpdate_t lobbyDataUpdate)
    {
        if (CurrentLobby != null && CurrentLobby.Value.Id.m_SteamID != lobbyDataUpdate.m_ulSteamIDLobby)
        {
            return;
        } 
        var lobbyId = new CSteamID(lobbyDataUpdate.m_ulSteamIDLobby);
        var lobbyMemberNum = SteamMatchmakingHelper.GetNumLobbyMembers(lobbyId);
        lobbyMembers.Clear();
        lobbyMembersNetworkId.Clear();
        for (int i = 0; i < lobbyMemberNum; i++)
        {
            var memberId = SteamMatchmakingHelper.GetLobbyMemberByIndex(lobbyId, i);
            var lobbyMemberData = SteamMatchmakingHelper.GetLobbyMemberData(lobbyId, memberId, SteamHelper.MemberIDKey);
            if (!lobbyMemberData.IsNullOrSpace())
            {
                int memberNetworkId = Convert.ToInt32(lobbyMemberData);
                lobbyMembers.Add(memberNetworkId, memberId);
                lobbyMembersNetworkId.Add(memberId, memberNetworkId);
            }
        }
    }

    [ShowInInspector][ReadOnly]
    Dictionary<int, CSteamID> lobbyMembers = new();
    [ShowInInspector][ReadOnly]
    Dictionary<CSteamID, int> lobbyMembersNetworkId = new();

    public CSteamID GetMemberSteamId(int networkId)
    {
        return lobbyMembers.GetValueOrDefault(networkId);
    }

    public int GetMemberNetworkId(CSteamID memberId) => lobbyMembersNetworkId.GetValueOrDefault(memberId);

    // 写个单例
    private static SteamAPIManager _instance;

    public static SteamAPIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SteamAPIManager").AddComponent<SteamAPIManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }
}