using System;
using FishNet.Object;
using UnityEngine;
using YuoTools.Main.Ecs;
using Random = UnityEngine.Random;

public class PlayerClientManager : NetworkBehaviour
{
    public GameObject playerPrefab;
    public GameObject cubeManagerPrefab;  // 测试写的
    
    // todo：这里持有玩家信息，创建时可以去取到信息。
    // 到时候客户端逻辑写在这里面，可以拆出去封装成事件 
    // 给一个创建的逻辑，用于进入后，创建玩家  
    // 进去之后，UI那边发事件，这边进行监听注册；  定义几个事件出来，比如游戏开始事件。 
    // 当房主点击开始后，发送一个事件出来 
    // todo:缺少事件管理器，需要补充一个。
    // 所有人的得分都在他们的头顶，当有人吃到方块后，上报给服务器，服务器广播给所有人，所有人更新自己的得分。
    NetPlayerComponent playerComponent;
    
    public override void OnStartClient()
    {
        base.OnStartClient(); 
        playerComponent = YuoWorld.Scene.AddChild<NetPlayerComponent>();
        playerComponent.player = this;
        playerComponent.onInitAction = OnGameEnter;   // 其实这里应该是开启游戏的事件，这里只是一个示例
        // 可以抛一个事件出去，表示该客户端已经初始化完成了 
    }


    public void OnGameEnter()
    {
        if (!IsOwner) return;
        var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
        if (async != null)
            async.completed += _ => { S_OnGameEnter(); }; 
    }
    

    [ServerRpc]
    private void S_OnGameEnter()
    { 
        var player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        
        Debug.Log($"绑定owner ！ OwnerID：{OwnerId}");
        Spawn(player,Owner);  
        
        CreateCubeManager();
    }

    [Server]  // 测试写的
    private void CreateCubeManager()
    {
        var cubeMgr = Instantiate(cubeManagerPrefab);
        Spawn(cubeMgr,Owner);  
    }
    
    private void OnDestroy()
    {
        playerComponent?.Entity?.Destroy();
    } 
}