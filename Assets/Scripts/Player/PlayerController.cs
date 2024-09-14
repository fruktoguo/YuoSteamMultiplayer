using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        if (!IsOwner) return;

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, moveY));

        if (IsServer)
        {
            UpdatePositionServerRpc(transform.position);
        }
        else
        {
            UpdatePositionClientRpc(transform.position);
        }
    }

    [ServerRpc]
    private void UpdatePositionServerRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}