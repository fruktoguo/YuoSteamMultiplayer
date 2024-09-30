using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.Events;
using YuoTools;

public class PlayerReadySync : NetworkBehaviour
{
    public readonly SyncVar<bool> syncValue = new();

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
        $"{gameObject.name} Ready: {next}".Log();
        onReadyChange?.Invoke(next);
    }

    public void SetValue(bool value)
    {
        ServerValueChangeRpc(value);
    }

    public bool GetValue()
    {
        return syncValue.Value;
    }

    [ServerRpc]
    void ServerValueChangeRpc(bool isReady)
    {
        $"服务器 收到 {gameObject.name} 消息 Ready {isReady}".Log();
        syncValue.Value = isReady;
    }
}