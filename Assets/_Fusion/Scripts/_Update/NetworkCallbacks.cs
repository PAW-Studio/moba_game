using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkCallbacks : NetworkBehaviour, INetworkRunnerCallbacks
{
    private int maxPlayers = 4;

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server.");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError($"Failed to connect to server: {reason}");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        NetworkManager nm = FindObjectOfType<NetworkManager>();
        FusionNetwork.GameManager GM = FusionNetwork.GameManager.Instance;
        if (nm._networkRunner.IsServer)
        {
            Debug.LogError(runner.AuthenticationValues.UserId);
            ServerPlayerData pd = new ServerPlayerData();
            pd.userID = player.PlayerId.ToString();
            pd.playerRef = player;
            pd.runner = runner;

            GM.players.Add(pd.userID, pd);

            Debug.LogError(GM.players.Count);
            Debug.LogError(maxPlayers);
            // Redirecting players into lobby
            if (GM.players.Count == maxPlayers)
            {
                try
                {
                    GM.StartTeamAllocation();
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            //FindObjectOfType<Demo>().Call_RPC();
            //Debug.Log("IsSERVER => CALLING RPC!!!!!!");
        }

        Debug.Log($"Player {player.PlayerId} joined the game.");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        NetworkManager nm = FindObjectOfType<NetworkManager>();
        FusionNetwork.GameManager GM = FusionNetwork.GameManager.Instance;
        if (nm._networkRunner.IsServer)
        {
            string tempID = player.PlayerId.ToString();
            if (!string.IsNullOrEmpty(tempID) && GM.players.ContainsKey(tempID))
            {
                GM.players.Remove(tempID);
            }
        }

        Debug.Log($"Player {player.PlayerId} left the game.");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason reason)
    {
        Debug.Log($"Server shutdown: {reason}");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Handle player input here
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.LogWarning($"Input missing for player {player.PlayerId}");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Session list updated.");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("Custom authentication response received.");
    }

    public void OnHostMigration(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Host migration in progress.");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("Reliable data received.");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("Scene loading started.");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene loading completed.");
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        //throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        //throw new NotImplementedException();
    }
}

[Serializable]
public class ServerPlayerData
{
    public string userID;
    public string playerName;
    public PlayerRef playerRef;
    public NetworkRunner runner;
}

[Serializable]
public class LobbyPlayerData
{
    public string userID;
    public string playerName;
    public int laneIndex;
    public int selectedCharacter;
    public float individualTimer;
}

[Serializable]
public class GameState
{
    public GameStateEnum state;
    public float commonTimer;
    public bool isFirstTeamSelecting;
    public List<LobbyPlayerData> Team_A = new List<LobbyPlayerData>();
    public List<LobbyPlayerData> Team_B = new List<LobbyPlayerData>();
}

[Serializable]
public enum GameStateEnum
{
    None,
    LaneAllocation,
    TeamSelection_A,
    TeamSelection_B,
}