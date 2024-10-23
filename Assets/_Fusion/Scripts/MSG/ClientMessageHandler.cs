using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientMessageHandler : NetworkBehaviour
{
    public TMP_Text text;

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ReceiveMessage(string message)
    {
        text.text = message;
        Debug.Log($"Received message from server: {message}");
    }
}

