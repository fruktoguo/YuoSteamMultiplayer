using System;
using FishNet.Object;
using UnityEngine;

public class PlayerClientManager : NetworkBehaviour
{
    // todo：这里持有玩家信息，创建时可以去取到信息。
    // 到时候客户端逻辑写在这里面，可以拆出去封装成事件
    
    
    // 给一个创建的逻辑，用于进入后，创建玩家
    
    
    // 进去之后，UI那边发事件，这边进行监听注册；  定义几个事件出来，比如游戏开始事件。

    // 当房主点击开始后，发送一个事件出来
    
    // todo:缺少事件管理器，需要补充一个。
    // 所有人的得分都在他们的头顶，当有人吃到方块后，上报给服务器，服务器广播给所有人，所有人更新自己的得分。
    
    private void Awake()
    {
        AddListener();

    }
    
    private void AddListener()
    {
        // todo：监听事件
        
    }
    
    private void RemoveListener()
    {
        // todo：移除监听
    }
    
    
    private void OnDestroy()
    {
        RemoveListener();
    } 
}