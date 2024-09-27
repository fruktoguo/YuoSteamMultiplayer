using SteamAPI.SteamHelper;
using Steamworks;
using YuoTools;
using YuoTools.Extend;
using YuoTools.Main.Ecs;

public class NetPlayerComponent : YuoComponent
{
    public PlayerClientManager player;
    public CSteamID steamID;
}

public class NetPlayerComponentStartSystem : YuoSystem<NetPlayerComponent>, IStart
{
    protected override async void Run(NetPlayerComponent component)
    {
        var player = component.player;
        component.AddComponent<EntitySelectComponent>().SelectGameObject = player.gameObject;
        await YuoWait.WaitFrameAsync(1);
        if (SteamAPIManager.Instance.CurrentLobby != null && player.IsOwner)
        {
            SteamMatchmakingHelper.SetLobbyMemberData(SteamAPIManager.Instance.CurrentLobby.Value,
                SteamHelper.MemberIDKey,
                SteamAPIManager.Instance.networkManager.ClientManager.Connection.ClientId.ToString());
        }

        await YuoWait.WaitFrameAsync(1);

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
            $"玩家 {component.Entity.EntityName}({steamID}) 加入房间".Log();
        }

        component.Entity.RunSystem<INetPlayerAwake>();
    }
}