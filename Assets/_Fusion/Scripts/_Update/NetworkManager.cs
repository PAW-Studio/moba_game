using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    //[SerializeField] private NetworkRunner _networkRunnerPrefab;

    public NetworkRunner _networkRunner;

    public int charecterIndex;
    public int charSelectionTimer;
    public bool IsServer;

    private void Start()
    {
        if (IsServer)
            StartServer();
        else
            StartClient();
    }

    // Starts the server
    public async void StartServer()
    {
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;

        _networkRunner.AddCallbacks(new NetworkCallbacks());

        var result = await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Server,      // Dedicated server mode
            SessionName = "DedicatedServer",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),  // Load current scene
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() // Manages the scene
        });

        if (result.Ok)
        {
            Debug.LogError("Server started successfully.");
        }
        else
        {
            Debug.LogError($"Failed to start server: {result.ShutdownReason}");
        }
    }

    // Connect as a client
    public async void StartClient()
    {
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;

        _networkRunner.AddCallbacks(new NetworkCallbacks());

        // Set AuthenticationValues
        var authValues = new AuthenticationValues();
        authValues.UserId = Random.Range(0,10000000).ToString();
        Debug.LogError(authValues.UserId);
        authValues.AddAuthParameter("playerName", RandomString(6));

        var result = await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,  // Client mode
            SessionName = "DedicatedServer",
            AuthValues = authValues,
            //Address = NetAddress.FromString(address), // Server address
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            Debug.Log("Client connected successfully.");
        }
        else
        {
            Debug.LogError($"Failed to connect as client: {result.ShutdownReason}");
        }
    }

    // Shutdown the server or client
    public void Shutdown()
    {
        if (_networkRunner != null)
        {
            _networkRunner.Shutdown();
            Debug.Log("Network runner shutdown.");
        }
    }

    private System.Random random = new System.Random();
    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
