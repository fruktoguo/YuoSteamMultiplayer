using System;
using FishNet.Object;
using Game.Event;
using Game.Manager;
using Steamworks;
using UniFramework.Event;
using UnityEngine;
using YuoTools;
using YuoTools.Main.Ecs;
using Random = UnityEngine.Random;

public class PlayerClientManager : NetworkBehaviour
{
    public GameObject playerPrefab;
    public GameObject cubeManagerPrefab;  // 测试写的
    public CSteamID steamID;
    
    NetPlayerComponent playerComponent;
    
    public override void OnStartClient()
    {
        base.OnStartClient(); 
        playerComponent = YuoWorld.Scene.AddChild<NetPlayerComponent, PlayerClientManager>(this);
        AddListeners();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        RemoveListeners();
    }

    private void AddListeners()
    {
        UniEvent.AddListener<S2C_StartEvent>(OnGameStart);
    }

    private void RemoveListeners()
    {
        UniEvent.RemoveListener<S2C_StartEvent>(OnGameStart); 
    }

    private void OnGameStart(IEventMessage startEvent)
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
        PlayerManager.UnRegisterPlayer(this);
        playerComponent?.Entity?.Destroy();
    } 
}