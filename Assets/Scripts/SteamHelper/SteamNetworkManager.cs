using ET;
using UnityEngine;
using Steamworks;
using Unity.Netcode;
using UnityEngine.Events;
using YuoTools;

public class SteamNetworkManager : SingletonMono<SteamNetworkManager>
{
    public bool isInit;

    private void Start()
    {
        if (!SteamAPI.Init())
        {
            Debug.LogError("Steam API 初始化失败！");
        }
    }

    private void OnDestroy()
    {
        SteamAPI.Shutdown();
    }

    public GameObject Scene;

    public void EnterScene()
    {
        Scene.Show();
    }

    public async ETTask<bool> CheckNATStatus(UnityAction<string> message)
    {
        SteamNetworkingUtils.GetRelayNetworkStatus(out var status);

        // Debug.Log($"NAT类型检查状态: {status.m_eAvail}");
        while (status.m_eAvail == ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Attempting)
        {
            message?.Invoke("正在尝试确定NAT类型...");
            await YuoWait.WaitTimeAsync(1);
            SteamNetworkingUtils.GetRelayNetworkStatus(out status);
        }

        switch (status.m_eAvail)
        {
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Attempting:
                message?.Invoke("正在尝试确定NAT类型...");
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Current:
                CheckOver();
                message?.Invoke("NAT类型已确定，可以进行P2P连接");
                return true;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Retrying:
                message?.Invoke("NAT类型确定失败，正在重试...");
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_NeverTried:
                message?.Invoke("尚未尝试确定NAT类型");
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Failed:
                CheckOver();
                message?.Invoke("确定NAT类型失败，可能需要使用中继服务器");
                break;
            default:
                message?.Invoke($"未知的NAT状态: {status.m_eAvail}");
                break;
        }

        return false;
    }

    void CheckOver()
    {
        InitializeNetworkManager();
    }

    public GameObject PlayerPrefab;

    private void InitializeNetworkManager()
    {
        NetworkManager networkManager = gameObject.AddComponent<NetworkManager>();
        networkManager.NetworkConfig = new NetworkConfig
        {
            NetworkTransport = gameObject.AddComponent<SteamP2PTransport>(),
            PlayerPrefab = PlayerPrefab,
        };

        isInit = true;

        EnterScene();
    }

    public void CreateRoom()
    {
        // 启动Netcode Host
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("主机启动成功。");
        }
        else
        {
            Debug.LogError("主机启动失败。");
        }
    }

    public void JoinRoom()
    {
        // 启动Netcode Client
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("客户端启动成功。");
        }
        else
        {
            Debug.LogError("客户端启动失败。");
        }
    }
}