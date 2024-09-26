using System;
using FishNet.Managing;
using HeathenEngineering.SteamworksIntegration;
using Steamworks;
using UnityEngine;

public class SteamAPIManager : SteamworksBehaviour
{
    public static ulong LocalUserSteamID;
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