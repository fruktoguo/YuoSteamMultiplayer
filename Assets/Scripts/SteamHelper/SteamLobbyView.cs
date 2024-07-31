using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using System.Collections.Generic;

public class SteamLobbyView : MonoBehaviour
{
    public Button createLobbyButton;
    public Button findLobbiesButton;
    public Button lobbyItem;
    public Transform lobbyListContainer;

    public Transform lobbyView;
    public Transform roomView;

    public SteamLobbyManager steamLobbyManager;

    void Start()
    {
        steamLobbyManager.OnLobbyListUpdated += UpdateLobbyList;
        steamLobbyManager.OnLobbyCreatedCallback += OnLobbyCreated;
        steamLobbyManager.OnLobbyEnteredCallback += OnLobbyEntered;
        steamLobbyManager.OnLobbyExitedCallback += OnLobbyExited; // 订阅退出事件

        createLobbyButton.onClick.AddListener(CreateLobby);
        findLobbiesButton.onClick.AddListener(FindLobbies);
    }

    void CreateLobby()
    {
        steamLobbyManager.CreateLobby();
    }

    void FindLobbies()
    {
        steamLobbyManager.FindAllLobbies();
    }

    void UpdateLobbyList(string lobbyListString)
    {
        // 清空现有的大厅列表
        foreach (Transform child in lobbyListContainer)
        {
            Destroy(child.gameObject);
        }

        // 获取大厅列表
        List<CSteamID> lobbies = steamLobbyManager.GetLobbyList();
        foreach (CSteamID lobbyID in lobbies)
        {
            Button lobbyButton = Instantiate(lobbyItem, lobbyListContainer);
            lobbyButton.gameObject.SetActive(true);
            Text lobbyText = lobbyButton.GetComponentInChildren<Text>();
            lobbyText.text = $"大厅ID: {lobbyID}";

            lobbyButton.onClick.AddListener(() => JoinLobby(lobbyID));
        }
    }

    void JoinLobby(CSteamID lobbyID)
    {
        steamLobbyManager.JoinLobby(lobbyID);
    }

    void OnLobbyCreated(bool success, string message)
    {
        if (success)
        {
            // 隐藏大厅视图，显示房间视图
            lobbyView.gameObject.SetActive(false);
            roomView.gameObject.SetActive(true);

            // 设置房间视图的lobbyView引用
            SteamRoomView roomViewScript = roomView.GetComponent<SteamRoomView>();
            roomViewScript.lobbyView = lobbyView;
        }
    }

    void OnLobbyEntered(bool success, string message)
    {
        // Debug.Log(message);
        if (success)
        {
            // 隐藏大厅视图，显示房间视图
            lobbyView.gameObject.SetActive(false);
            roomView.gameObject.SetActive(true);

            // 设置房间视图的lobbyView引用
            SteamRoomView roomViewScript = roomView.GetComponent<SteamRoomView>();
            roomViewScript.lobbyView = lobbyView;
            
            roomViewScript.EnterRoom();
        }
    }

    void OnLobbyExited()
    {
        // 显示大厅视图，隐藏房间视图
        lobbyView.gameObject.SetActive(true);
        roomView.gameObject.SetActive(false);
    }
}