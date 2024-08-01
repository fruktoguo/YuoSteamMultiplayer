using UnityEngine;
using Steamworks;
using Unity.Netcode;
using YuoTools;

public class SteamNetworkManager : SingletonMono<SteamNetworkManager>
{
    public bool isInit;

    private void Start()
    {
        if (!SteamAPI.Init())
        {
            Debug.LogError("Steam API 初始化失败！");
            return;
        }

        InvokeRepeating(nameof(CheckNATStatus), 1f, 1f); // 每5秒检查一次NAT状态
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

    private void CheckNATStatus()
    {
        SteamNetworkingUtils.GetRelayNetworkStatus(out var status);

        // Debug.Log($"NAT类型检查状态: {status.m_eAvail}");

        switch (status.m_eAvail)
        {
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Attempting:
                Debug.Log("正在尝试确定NAT类型...");
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Current:
                Debug.Log("NAT类型已确定，可以进行P2P连接");
                CheckOver();
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Retrying:
                Debug.Log("NAT类型确定失败，正在重试...");
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_NeverTried:
                Debug.Log("尚未尝试确定NAT类型");
                break;
            case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Failed:
                Debug.Log("确定NAT类型失败，可能需要使用中继服务器");
                CheckOver();
                break;
            default:
                Debug.Log($"未知的NAT状态: {status.m_eAvail}");
                break;
        }
    }

    void CheckOver()
    {
        // 当NAT类型确定后，停止定期检查，并执行连接操作
        CancelInvoke(nameof(CheckNATStatus));
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