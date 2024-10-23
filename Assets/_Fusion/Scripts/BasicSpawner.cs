using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using System.Linq;

public enum Team
{
    TeamA,
    TeamB
}

public enum Lanes
{
    Lane1,
    Lane2,
    Lane3,
    Lane4,
    Lane5
}

[System.Serializable]
public class PlayerSpawnPoints
{
    public int index;
    public bool isAvilable = true;
    public Transform point;
    public Team team;
}
[System.Serializable]
public class LaneInfo
{
    public string playerName;
    public Lanes lane;
    public bool isOcupied = false;
}

public class BasicSpawner : Singleton<BasicSpawner>, INetworkRunnerCallbacks
{
    public NetworkRunner _runner;

    public GameObject matchMakingScreen;
    public GameObject characterScreen;
    public GameObject loadingObject;
    public GameObject findMatchScreen;
    public GameObject mainMenu;

    public Toggle serverToggle;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public TMP_Text text;
    public FixedJoystick joyStick;
    // Specify a unique session name
    public string sessionName = "MyGameSession";
    public TMP_InputField inputField;

    public List<PlayerSpawnPoints> playerSpawnPoints = new List<PlayerSpawnPoints>();


    public List<LaneInfo> laneInfos = new List<LaneInfo>();
    public int PlayerCount = 0;
    public int teamAPlayers = 0;
    public int teamBPlayers = 0;


    public ServerMessageHandler serverMessageHandler;
    public PlayerSelectionManager playerSelectionManager;
    public NetworkManager networkManager;
    public int tempTimeValue;

    public int maxTimerCount = 60;
    public bool tempValue = false;
    #region UNITY_METHODS
    private void Start()
    {
#if LOCAL_SERVER
        inputField.text = sessionName;
        StartServer();
#endif
    }



    #endregion

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),

        });
    }

    public PlayerSpawnPoints GetSpawnPointTeamA()
    {
        PlayerSpawnPoints points = null;
        foreach (var item in playerSpawnPoints)
        {
            if (item.isAvilable && item.team == Team.TeamA)
            {
                points = item;
                item.isAvilable = false;
                break;
            }
        }
        return points;
    }

    public PlayerSpawnPoints GetSpawnPointTeamB()
    {
        PlayerSpawnPoints points = null;
        foreach (var item in playerSpawnPoints)
        {
            if (item.isAvilable && item.team == Team.TeamB)
            {
                points = item;
                item.isAvilable = false;
                break;
            }
        }
        return points;
    }
    public void HostGame()
    {
        StartServer();
        //  StartGame(GameMode.Host);
        loadingObject.SetActive(true);
        // matchMakingScreen.SetActive(false);

    }

    public void JoinGame()
    {
        loadingObject.SetActive(true);

#if LOCAL_CLIENT
        inputField.text = sessionName;
        StartClient();
#endif

    }

    public void ServerClientCreate()
    {
        if (serverToggle.isOn)
        {
            loadingObject.SetActive(true);
            inputField.text = sessionName;
            StartServer();
        }
        else
        {

            loadingObject.SetActive(true);
            inputField.text = sessionName;
            StartClient();
        }
    }

    void StartServer()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        sessionName = inputField.text;
        _runner.AddCallbacks(new NetworkCallbacks());
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        StartGameArgs startGameArgs = new StartGameArgs()
        {
            GameMode = GameMode.Server,
            Scene = scene,
            SessionName = sessionName // Set the session name
        };
        _runner.StartGame(startGameArgs).ContinueWith(OnStartGameCompleted);

    }

    void StartClient()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(new NetworkCallbacks());
        sessionName = inputField.text;
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        StartGameArgs startGameArgs = new StartGameArgs()
        {
            GameMode = GameMode.Client,
            Scene = scene,
            SessionName = sessionName // Use the same session name
        };

        _runner.StartGame(startGameArgs).ContinueWith(OnStartGameCompleted);
    }

    // Callback to handle the result of starting the game
    void OnStartGameCompleted(Task<StartGameResult> task)
    {
        if (task.Result.Ok)
        {
            Debug.Log("Game started successfully");
        }
        else
        {
            Debug.LogError($"Failed to start game: {task.Result.ErrorMessage}");
        }
    }


    IEnumerator GameStartTimer()
    {
        int tempTime = 0;
        while (maxTimerCount > tempTime)
        {
            Debug.Log(tempTime);
            yield return new WaitForSeconds(1);
            tempTime++;
            networkManager.charSelectionTimer = tempTime;
        }
        gameObject.SetActive(false);

    }
    private void Update()
    {
        if (tempValue)
        {
            tempTimeValue = networkManager.charSelectionTimer;
        }
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer : " + runner);
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed : " + runner);

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest : " + runner);
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse : " + runner);

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("OnCustomAuthenticationResponse : " + runner);
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnCustomAuthenticationResponse : " + runner);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        // Get input from the joystick
        Vector2 joystickInput = joyStick.Direction;

        // Movement
        if (joystickInput.y > 0.1f)
            data.direction += Vector3.forward;

        if (joystickInput.y < -0.1f)
            data.direction += Vector3.back;

        if (joystickInput.x < -0.1f)
            data.direction += Vector3.left;

        if (joystickInput.x > 0.1f)
            data.direction += Vector3.right;


        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing : " + runner);
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("OnObjectEnterAOI : " + runner);
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("OnObjectExitAOI : " + runner);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);

            Team team;
            Lanes lanes;
            if (PlayerCount % 2 == 0)
            {
                lanes = laneInfos[teamAPlayers].lane;
                teamAPlayers++;
                team = Team.TeamA;
                spawnPosition = GetSpawnPointTeamA().point.position;
            }
            else
            {
                lanes = laneInfos[teamBPlayers].lane;
                teamBPlayers++;
                team = Team.TeamB;
                spawnPosition = GetSpawnPointTeamB().point.position;
            }

            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            RoomPlayer roomPlayer = networkPlayerObject.GetComponent<RoomPlayer>();

            roomPlayer.id = PlayerCount;
            roomPlayer.teamName = team;
            roomPlayer.currentLane = lanes;
            roomPlayer.charSelectionTimer = 10;

            PlayerCount++;
            StartCoroutine(GameStartTimer());
            _spawnedCharacters.Add(player, networkPlayerObject);
            playerSelectionManager.StartPlayerSelection(runner);

            Debug.Log("---------------------->>> " + (runner.SessionInfo.PlayerCount));

            if (PlayerCount == (runner.SessionInfo.PlayerCount - 1))
            {
                tempValue = true;
                Debug.Log("----------------------- >>>>> ");
                
            }
        }
    }

    public TMP_Text _messages;

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (messageSource == _runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

        Debug.Log("MESSAGE : " + message);
        _messages.text += message;
    }
    // Define an RPC method that will be called on all clients
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RpcSendMessage(string message)
    {
        Debug.Log($"Message from server: {message}");
        // You can add more functionality here to handle the message on the client side
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerLeft : " + runner);

        if (runner.IsServer)
        {
            PlayerCount--;
        }
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log("OnReliableDataProgress : " + runner);

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log("OnReliableDataReceived : " + runner);
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.IsServer)
        {
            Debug.Log("OnSceneLoadDone " + runner);
            matchMakingScreen.SetActive(false);
            characterScreen.SetActive(false);
            findMatchScreen.SetActive(false);
            mainMenu.SetActive(false);

        }

        if (runner.IsClient)
        {
            matchMakingScreen.SetActive(false);
            characterScreen.SetActive(true);
            findMatchScreen.SetActive(true);
        }
        loadingObject.SetActive(false);
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadStart : " + runner);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("OnSessionListUpdated : " + runner);

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("OnShutdown : " + runner);

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("OnUserSimulationMessage : " + runner);
    }

}
