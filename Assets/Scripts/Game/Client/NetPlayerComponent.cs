using System;
using SteamAPI.SteamHelper;
using Steamworks;
using UniFramework.Event;
using YuoTools;
using YuoTools.Extend;
using YuoTools.Main.Ecs;

public class NetPlayerComponent : YuoComponent
{
    public PlayerClientManager player;
    public CSteamID steamID;
    public Action onInitAction;
}

public class NetPlayerComponentStartSystem : YuoSystem<NetPlayerComponent>, IStart
{
    NetPlayerComponent _component;
    
    /// <summary>
    /// 说明：这里的链路不对，
    /// </summary>
    /// <param name="component"></param>
    protected override async void Run(NetPlayerComponent component)
    {
        _component = component;
        var player = component.player;
        component.AddComponent<EntitySelectComponent>().SelectGameObject = player.gameObject; 
        if (SteamAPIManager.Instance.CurrentLobby != null && player.IsOwner)
        {
            SteamMatchmakingHelper.SetLobbyMemberData(SteamAPIManager.Instance.CurrentLobby.Value,
                SteamHelper.MemberIDKey,
                SteamAPIManager.Instance.networkManager.ClientManager.Connection.ClientId.ToString());
        } 

        CSteamID steamID = default;
        while (steamID == default)
        {
            steamID = SteamAPIManager.Instance.GetMemberSteamId(player.OwnerId);
            await YuoWait.WaitFrameAsync(10);
        }

        if (steamID != default)
        {
            component.Entity.EntityName = $"{SteamFriendsHelper.GetFriendPersonaName(steamID)}";
            component.player.gameObject.name = $"{SteamFriendsHelper.GetFriendPersonaName(steamID)}({steamID})";
            component.steamID = steamID; 
            $"玩家 {component.Entity.EntityName}({steamID})--ownerID:{player.OwnerId} 加入房间".Log();
        } 
        component.onInitAction?.Invoke();
        component.Entity.RunSystem<INetPlayerAwake>();  
    }  
}