using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages network-related functionality, including starting a server, connecting as a client, and shutting down.
/// </summary>
public class NetworkManager : MonoBehaviour
{
    // NetworkRunner instance used for managing network operations
    public NetworkRunner _networkRunner;

    // Properties related to character selection
    public int characterIndex;       // Character index for selection
    public int charSelectionTimer;   // Timer for character selection

    public bool IsServer;            // Indicates if this instance is running as a server

    private System.Random random = new System.Random(); // Random generator for unique UserIDs

    private void Awake()
    {
        // Initialization logic if needed
    }

    /// <summary>
    /// Starts the server to host a game session.
    /// </summary>
    public async void StartServer()
    {
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;

        // Attach callback handler for network events
        _networkRunner.AddCallbacks(new NetworkCallbacks());

        // Start the game in server mode
        var result = await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Server,      // Dedicated server mode
            SessionName = "DedicatedServer",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),  // Load the active scene
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()   // Scene management
        });

        // Log the outcome of starting the server
        if (result.Ok)
        {
            Debug.LogError("Server started successfully.");
        }
        else
        {
            Debug.LogError($"Failed to start server: {result.ShutdownReason}");
        }
    }

    /// <summary>
    /// Connects as a client to an existing game session.
    /// </summary>
    public async void StartClient()
    {
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;

        // Attach callback handler for network events
        _networkRunner.AddCallbacks(new NetworkCallbacks());

        // Set up authentication values for the client
        var authValues = new AuthenticationValues
        {
            UserId = Random.Range(0, 10000000).ToString() // Generate a unique UserId
        };
        Debug.LogError($"Client UserID: {authValues.UserId}");
        authValues.AddAuthParameter("playerName", GenerateRandomString(6)); // Generate a random player name

        // Start the game in client mode
        var result = await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,  // Client mode
            SessionName = "DedicatedServer",
            AuthValues = authValues,    // Authentication parameters
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() // Scene management
        });

        // Log the outcome of the connection attempt
        if (result.Ok)
        {
            Debug.Log("Client connected successfully.");
        }
        else
        {
            Debug.LogError($"Failed to connect as client: {result.ShutdownReason}");
        }
    }

    /// <summary>
    /// Shuts down the network runner, stopping all network activity.
    /// </summary>
    public void Shutdown()
    {
        if (_networkRunner != null)
        {
            _networkRunner.Shutdown();
            Debug.Log("Network runner shutdown.");
        }
    }

    /// <summary>
    /// Generates a random alphanumeric string of the specified length.
    /// </summary>
    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
