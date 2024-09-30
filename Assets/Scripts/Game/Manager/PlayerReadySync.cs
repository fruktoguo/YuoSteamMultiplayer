using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.Events;
using YuoTools;

public class PlayerReadySync : NetworkBehaviour
{
    public readonly SyncVar<bool> syncValue = new();

    public bool IsReady;

    public UnityEvent<bool> onReadyChange = new();

    private void Awake()
    {
        syncValue.OnChange += OnValueChange;
    }

    private void OnDestroy()
    {
        syncValue.OnChange -= OnValueChange;
    }

    private void OnValueChange(bool prev, bool next, bool asServer)
    {
        onReadyChange?.Invoke(next);
    }

    public void SetValue(bool value)
    {
        IsReady = value;
        ServerValueChangeRpc();
    }

    public bool GetValue()
    {
        return syncValue.Value;
    }

    [ServerRpc]
    void ServerValueChangeRpc()
    {
        syncValue.Value = IsReady;
    }
}