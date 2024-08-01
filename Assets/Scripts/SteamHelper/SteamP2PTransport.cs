using System;
using UnityEngine;
using Unity.Netcode;
using Steamworks;

public class SteamP2PTransport : NetworkTransport
{
    private ulong serverClientId = 0;

    public override void Send(ulong clientId, ArraySegment<byte> data, NetworkDelivery delivery)
    {
        try
        {
            if (data.Count <= 0)
            {
                Debug.LogError("尝试发送空数据包，忽略...");
                return;
            }

            // 添加日志记录数据包大小
            Debug.Log($"发送数据包大小: {data.Count} 字节");

            EP2PSend sendType = delivery == NetworkDelivery.Reliable ? EP2PSend.k_EP2PSendReliable : EP2PSend.k_EP2PSendUnreliable;
            bool success = SteamNetworking.SendP2PPacket((CSteamID)clientId, data.Array, (uint)data.Count, sendType);
            if (!success)
            {
                Debug.LogError($"发送P2P数据包失败，客户端ID: {clientId}");
            }
            else
            {
                Debug.Log($"成功发送P2P数据包，客户端ID: {clientId}, 数据大小: {data.Count}字节");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"发送P2P数据包时发生异常：{ex.Message}");
        }
    }

    public override NetworkEvent PollEvent(out ulong clientId, out ArraySegment<byte> payload, out float receiveTime)
    {
        try
        {
            uint messageSize;
            if (SteamNetworking.IsP2PPacketAvailable(out messageSize))
            {
                if (messageSize <= 0)
                {
                    Debug.LogError("收到的数据包大小无效，忽略...");
                    clientId = 0;
                    payload = new ArraySegment<byte>();
                    receiveTime = 0;
                    return NetworkEvent.Nothing;
                }

                byte[] data = new byte[messageSize];
                uint bytesRead;
                CSteamID remoteId;
                if (SteamNetworking.ReadP2PPacket(data, messageSize, out bytesRead, out remoteId))
                {
                    // 添加日志记录数据包大小
                    Debug.Log($"接收数据包大小: {bytesRead} 字节");

                    clientId = (ulong)remoteId;
                    payload = new ArraySegment<byte>(data, 0, (int)bytesRead);
                    receiveTime = Time.realtimeSinceStartup;
                    Debug.Log($"收到来自客户端ID: {clientId}的数据包，大小: {bytesRead}字节");
                    return NetworkEvent.Data;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"轮询P2P数据包时发生异常：{ex.Message}");
        }

        clientId = 0;
        payload = new ArraySegment<byte>();
        receiveTime = 0;
        return NetworkEvent.Nothing;
    }

    public override bool StartClient()
    {
        Debug.Log("启动客户端...");
        if (!SteamAPI.Init())
        {
            Debug.LogError("初始化Steam API失败。");
            return false;
        }
        Debug.Log("客户端启动成功，Steam API初始化完成。");
        return true;
    }

    public override bool StartServer()
    {
        Debug.Log("启动服务器...");
        if (!SteamAPI.Init())
        {
            Debug.LogError("初始化Steam API失败。");
            return false;
        }
        serverClientId = (ulong)SteamUser.GetSteamID();
        Debug.Log($"服务器启动成功，服务器客户端ID: {serverClientId}");
        return true;
    }

    public override void DisconnectRemoteClient(ulong clientId)
    {
        Debug.Log($"断开远程客户端连接，客户端ID: {clientId}");
        SteamNetworking.CloseP2PSessionWithUser((CSteamID)clientId);
    }

    public override void DisconnectLocalClient()
    {
        Debug.Log("断开本地客户端连接...");
        SteamNetworking.CloseP2PSessionWithUser(SteamUser.GetSteamID());
    }

    public override ulong GetCurrentRtt(ulong clientId)
    {
        Debug.Log($"获取客户端ID: {clientId}的当前RTT");
        // 这里可以实现实际的RTT计算逻辑，例如发送ping消息并测量响应时间
        return 50; // 这里使用固定值作为占位符
    }

    public override void Shutdown()
    {
        // Debug.Log("关闭传输...");
        // SteamAPI.Shutdown();
        // Debug.Log("传输已关闭，Steam API已关闭。");
    }

    public override void Initialize(NetworkManager networkManager)
    {
        Debug.Log("初始化传输...");
        // 可以在这里初始化与networkManager相关的设置
        Debug.Log("传输初始化完成。");
    }

    public override ulong ServerClientId => serverClientId;
}