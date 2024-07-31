using System;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using System.Collections.Generic;
using YuoTools;

public class SteamRoomView : MonoBehaviour
{
    public Transform messageListContainer;
    public GameObject messageItemPrefab;
    public InputField messageInputField;
    public Button sendMessageButton;
    public Button backButton; // 新增返回按钮

    public SteamLobbyManager steamLobbyManager;

    public Transform lobbyView; // 新增大厅视图引用

    private void Start()
    {
        sendMessageButton.onClick.AddListener(SendMessage);
        backButton.onClick.AddListener(BackToLobby); // 绑定返回按钮点击事件
        steamLobbyManager.OnChatRoomUpdated += UpdateChatRoom;
    }

    private void SendMessage()
    {
        string message = messageInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            // 发送消息给所有成员
            List<CSteamID> lobbyMembers = steamLobbyManager.GetPlayerList();
            foreach (CSteamID memberID in lobbyMembers)
            {
                Debug.Log($"发送消息给成员ID: {memberID}");
                steamLobbyManager.SendP2PRequest(memberID, message);
            }

            messageInputField.text = "";
        }
    }
    
    public void EnterRoom()
    {
        // 清空现有的消息列表
        foreach (Transform child in messageListContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void BackToLobby()
    {
        // 调用退出大厅的方法
        steamLobbyManager.ExitLobby();
    }

    public void AddMessage(string message)
    {
        GameObject messageItem = Instantiate(messageItemPrefab, messageListContainer);
        Text text = messageItem.GetComponentInChildren<Text>();
        text.text = message;
        messageItem.SetActive(true);
    }
    
    private void UpdateChatRoom(string chatRoom)
    {
        // 解析并显示新的消息
        string[] messages = chatRoom.Split('\n');
        foreach (string msg in messages)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                AddMessage(msg);
            }
        }
    }
}