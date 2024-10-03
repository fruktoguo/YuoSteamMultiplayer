using System;
using Game.Manager;
using SteamAPI.SteamHelper;
using Steamworks;
using YuoTools;
using YuoTools.Core.Ecs;
using YuoTools.Extend;
using YuoTools.Main.Ecs;

public class NetPlayerComponent : YuoComponent, IComponentInit<PlayerClientManager>
{
    public PlayerClientManager player;
    public PlayerReadySync ready;
    public CSteamID steamID;

    public void ComponentInit(PlayerClientManager componentInitData)
    {
        player = componentInitData;
    }
}

public class NetPlayerComponentStartSystem : YuoSystem<NetPlayerComponent>, IStart
{
    /// <summary>
    /// 说明：这里的链路不对，
    /// </summary>
    /// <param name="component"></param>
    protected override async void Run(NetPlayerComponent component)
    {
        var player = component.player;
        component.AddComponent<EntitySelectComponent>().SelectGameObject = player.gameObject;
        component.ready ??= player.gameObject.GetComponent<PlayerReadySync>();
        if (SteamAPIManager.Instance.CurrentLobby != null && player.IsOwner)
        {
            SteamMatchmakingHelper.SetLobbyMemberData(SteamAPIManager.Instance.CurrentLobby.Value,
                SteamHelper.MemberIDKey,
                SteamAPIManager.Instance.networkManager.ClientManager.Connection.ClientId.ToString());
        }

        //等待玩家数据
        CSteamID steamID = default;
        while (steamID == default || steamID.m_SteamID == 0)
        {
            steamID = SteamAPIManager.Instance.GetMemberSteamId(player.OwnerId);
            await YuoWait.WaitFrameAsync(10);
        }

        await YuoWait.WaitFrameAsync();

        if (steamID != default && steamID.m_SteamID != 0)
        {
            player.steamID = steamID;
            PlayerManager.RegisterPlayer(player);
            component.Entity.EntityName = $"{SteamFriendsHelper.GetFriendPersonaName(steamID)}";
            component.player.gameObject.name = $"{SteamFriendsHelper.GetFriendPersonaName(steamID)}({steamID})";
            component.steamID = steamID;
            $"玩家 {component.Entity.EntityName}({steamID})--ownerID:{player.OwnerId} 加入房间".Log();
        }

        component.Entity.RunSystem<INetPlayerAwake>();
        await YuoWait.WaitUntilAsync(() => component.player.OwnerId.Log() != 0);
    }
}