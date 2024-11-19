using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

/// <summary>
/// Handles network-related callbacks and player management.
/// </summary>
public class NetworkCallbacks : NetworkBehaviour, INetworkRunnerCallbacks
{
    private int maxPlayers = 4;

    /// <summary>
    /// Called when the client successfully connects to the server.
    /// </summary>
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server.");
    }

    /// <summary>
    /// Called when the client fails to connect to the server.
    /// </summary>
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError($"Failed to connect to server: {reason}");
    }

    /// <summary>
    /// Called when a player joins the server.
    /// </summary>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        NetworkManager nm = FindObjectOfType<NetworkManager>();
        FusionNetwork.GameManager GM = FusionNetwork.GameManager.Instance;

        if (nm._networkRunner.IsServer)
        {
            Debug.LogError(runner.AuthenticationValues.UserId);

            // Create and add player data
            ServerPlayerData pd = new ServerPlayerData
            {
                userID = player.PlayerId.ToString(),
                playerRef = player,
                runner = runner
            };

            GM.players.Add(pd.userID, pd);

            Debug.LogError($"Player Count: {GM.players.Count}, Max Players: {maxPlayers}");

            // Start team allocation when the max number of players is reached
            if (GM.players.Count == maxPlayers)
            {
                try
                {
                    GM.StartTeamAllocation();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error starting team allocation: {e.Message}");
                }
            }
        }

        Debug.Log($"Player {player.PlayerId} joined the game.");
    }

    /// <summary>
    /// Called when a player leaves the server.
    /// </summary>
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

    /// <summary>
    /// Called when the server shuts down.
    /// </summary>
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

/// <summary>
/// Data representing a player connected to the server.
/// </summary>
[Serializable]
public class ServerPlayerData
{
    public string userID;
    public string playerName;
    public PlayerRef playerRef;
    public NetworkRunner runner;
}

/// <summary>
/// Data representing a player in the lobby.
/// </summary>
[Serializable]
public class LobbyPlayerData
{
    public string userID;
    public string playerName;
    public int laneIndex;
    public int selectedCharacter;
    public float individualTimer;
}

/// <summary>
/// Data representing the state of the game.
/// </summary>
[Serializable]
public class GameState
{
    public GameStateEnum state;
    public float commonTimer;
    public bool isFirstTeamSelecting;
    public List<LobbyPlayerData> Team_A = new List<LobbyPlayerData>();
    public List<LobbyPlayerData> Team_B = new List<LobbyPlayerData>();

    /// <summary>
    /// Get all players in the game from both teams.
    /// </summary>
    public IEnumerable<LobbyPlayerData> AllPlayers => Team_A.Concat(Team_B);

    /// <summary>
    /// Check if all players in a specific team have selected a character.
    /// </summary>
    public bool AllPlayersInTeamSelected(List<LobbyPlayerData> team)
    {
        return team.All(player => player.selectedCharacter != -1);
    }

    /// <summary>
    /// Check if all players across both teams have selected a character.
    /// </summary>
    public bool AllPlayersSelected()
    {
        return AllPlayers.All(player => player.selectedCharacter != -1);
    }
}

/// <summary>
/// Enum representing the different states of the game.
/// </summary>
[Serializable]
public enum GameStateEnum
{
    None,
    LaneAllocation,
    TeamSelection_A,
    TeamSelection_B,
}
