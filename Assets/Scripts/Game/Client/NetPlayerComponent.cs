using YuoTools;
using YuoTools.Extend;
using YuoTools.Main.Ecs;

public class NetPlayerComponent : YuoComponent
{
    public PlayerClientManager player;
}

public class NetPlayerComponentStartSystem : YuoSystem<NetPlayerComponent>, IStart
{
    protected override void Run(NetPlayerComponent component)
    {
        var player = component.player;
        component.AddComponent<EntitySelectComponent>().SelectGameObject = player.gameObject;
        component.Entity.EntityName = player.gameObject.name;
        
        $"玩家 {component.Entity.EntityName}({player.OwnerId}) 加入房间".Log();
    }
}