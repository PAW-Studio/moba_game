using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerMessageHandler : NetworkBehaviour
{
    public TMP_Text text;

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_SendMessageToAllClients(string message)
    {
        Debug.Log($"Message from server: {message}");
    }


  
    public void SendMessageFromServer(string message)
    {
        // Ensure this is the server before sending the message
        if (Runner.IsServer)
        {
            RPC_SendMessageToAllClients(message);
        }
    }
}
