namespace FusionNetwork
{
    using Fusion;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Manages the overall game state, team allocation, and player interactions.
    /// </summary>
    public class GameManager : NetworkBehaviour
    {
        #region PUBLIC_VAR
        public CameraFollow cameraFollow; // Reference to the camera follow component
        public int selectedCharacterIndex = 0; // Default selected character index
        public int maxPlayer = 2; // Maximum number of players allowed

        public static GameManager Instance; // Singleton instance of GameManager
        #endregion

        #region PRIVATE_VAR
        private GameState gameState; // Tracks the current game state
        private Coroutine commonRoundTimerRoutine = null; // Reference to the common round timer coroutine
        private Coroutine individualTimerRoutine = null; // Reference to the individual round timer coroutine
        #endregion

        /// <summary>
        /// Dictionary to store server-side player data.
        /// </summary>
        public Dictionary<string, ServerPlayerData> players = new Dictionary<string, ServerPlayerData>();

        #region UNITY_CALLBACKS

        private void Awake()
        {
            // Initialize the singleton instance
            Instance = this;
        }
        #endregion

        #region TEAM_ALLOCATION

        /// <summary>
        /// Starts the team allocation process.
        /// </summary>
        public void StartTeamAllocation()
        {
            StartCoroutine(AllocateTeams());
        }

        /// <summary>
        /// Allocates players into two teams and assigns initial lane indices.
        /// </summary>
        private IEnumerator AllocateTeams()
        {
            gameState = new GameState(); // Initialize game state

            int playerCounter = 0; // Counter to distribute players across teams

            Debug.LogError("Allocating Teams...");
            gameState.state = GameStateEnum.LaneAllocation;

            // Allocate players into Team A and Team B
            foreach (var playerData in players.Values)
            {
                LobbyPlayerData lobbyPlayer = new LobbyPlayerData
                {
                    userID = playerData.userID,
                    playerName = playerData.playerName,
                    laneIndex = playerCounter % 5, // Assign laneIndex in a round-robin fashion
                    selectedCharacter = -1, // Default: no character selected
                    individualTimer = 0f // Default: no timer initialized
                };

                // Alternate between Team A and Team B
                if (playerCounter % 2 == 0)
                {
                    gameState.Team_A.Add(lobbyPlayer);
                }
                else
                {
                    gameState.Team_B.Add(lobbyPlayer);
                }

                playerCounter++;
            }

            // Broadcast the updated game state
            RPC_SendGameState(JsonUtility.ToJson(gameState));

            yield return new WaitForSeconds(5f);

            // Transition to team selection phase
            gameState.state = GameStateEnum.TeamSelection_A;
            StartCoroutine(HandleTeamSelection(gameState, gameState.Team_A, 0)); // Start with Team A
        }
        #endregion

        #region TIMER_LOGIC

        /// <summary>
        /// Manages the common timer for a team's turn.
        /// </summary>
        private IEnumerator CommonRoundTimer(GameState gameState, int teamIndex)
        {
            gameState.commonTimer = 20f; // Set the timer (e.g., players per team * 10)
            RPC_SendGameState(JsonUtility.ToJson(gameState));

            while (gameState.commonTimer > 0)
            {
                yield return new WaitForSeconds(1f);
                gameState.commonTimer -= 1f;
                RPC_SendGameState(JsonUtility.ToJson(gameState));
                Debug.Log($"Common Timer: {gameState.commonTimer}");
            }

            if (gameState.commonTimer <= 0)
            {
                Debug.Log("Common timer expired!");
            }
        }

        /// <summary>
        /// Handles the individual timer for a single player's turn.
        /// </summary>
        private IEnumerator IndividualRoundTimer(LobbyPlayerData player, GameState gameState, List<LobbyPlayerData> team, int currentPlayerIndex)
        {
            player.individualTimer = 10f; // Set individual player timer
            RPC_SendGameState(JsonUtility.ToJson(gameState));

            while (player.individualTimer > 0)
            {
                yield return new WaitForSeconds(1f);
                player.individualTimer -= 1f;
                RPC_SendGameState(JsonUtility.ToJson(gameState));
                Debug.Log($"Player {player.playerName}'s Timer: {player.individualTimer}");

                if (player.selectedCharacter != -1)
                {
                    Debug.Log($"{player.playerName} selected a character!");
                    break;
                }
            }

            // Proceed to the next player or end the round
            currentPlayerIndex++;
            if (currentPlayerIndex < team.Count)
            {
                StartCoroutine(IndividualRoundTimer(team[currentPlayerIndex], gameState, team, currentPlayerIndex));
            }
            else
            {
                // Transition to the next phase
                if (gameState.state == GameStateEnum.TeamSelection_A)
                {
                    Debug.LogError("Team A completed selection!");
                    StopCoroutine(commonRoundTimerRoutine);

                    gameState.state = GameStateEnum.TeamSelection_B;
                    StartCoroutine(HandleTeamSelection(gameState, gameState.Team_B, 1)); // Move to Team B
                }
                else if (gameState.state == GameStateEnum.TeamSelection_B)
                {
                    Debug.LogError("Team B completed selection! Starting the game.");
                    StopCoroutine(commonRoundTimerRoutine);
                }
            }
        }
        #endregion

        #region GAME_FLOW

        /// <summary>
        /// Handles the selection phase for a given team.
        /// </summary>
        private IEnumerator HandleTeamSelection(GameState gameState, List<LobbyPlayerData> team, int teamIndex)
        {
            // Start the team's common timer
            commonRoundTimerRoutine = StartCoroutine(CommonRoundTimer(gameState, teamIndex));

            // Start individual player selection
            yield return StartCoroutine(IndividualRoundTimer(team[0], gameState, team, 0));
        }
        #endregion

        #region RPC_METHODS

        /// <summary>
        /// Server-side method for a player to select a character.
        /// </summary>
        [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
        public void RPC_SelectCharacter(string userID, int characterIndex)
        {
            Debug.LogError("Character selected!");
            if (FindObjectOfType<NetworkManager>()._networkRunner.IsServer)
            {
                var player = gameState.FindPlayer(userID);
                if (player != null)
                {
                    player.selectedCharacter = characterIndex;
                    Debug.Log($"Player {player.playerName} selected character {characterIndex}");
                }
            }
        }

        /// <summary>
        /// Broadcasts the current game state to all clients.
        /// </summary>
        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        public void RPC_SendGameState(string jsonData)
        {
            gameState = JsonUtility.FromJson<GameState>(jsonData);
            if (!FindObjectOfType<NetworkManager>()._networkRunner.IsServer)
            {
                PrintGameState(gameState);
            }
        }
        #endregion

        #region DEBUGGING

        /// <summary>
        /// Prints the game state for debugging purposes.
        /// </summary>
        private void PrintGameState(GameState gameState)
        {
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            System.Text.StringBuilder log = new System.Text.StringBuilder();
            log.AppendLine($"[{timestamp}] === GameState ===");
            log.AppendLine($"Common Timer: {gameState.commonTimer}");
            log.AppendLine($"Is First Team Selecting: {gameState.isFirstTeamSelecting}");
            log.AppendLine($"=== Team A ===");
            foreach (var player in gameState.Team_A) log.AppendLine(player.ToString());
            log.AppendLine($"=== Team B ===");
            foreach (var player in gameState.Team_B) log.AppendLine(player.ToString());
            log.AppendLine($"[{timestamp}] === End of GameState ===");
            Debug.Log(log.ToString());
        }
        #endregion
    }

    #region EXTENSIONS
    public static class GameStateExtensions
    {
        /// <summary>
        /// Finds a player in either team by their userID.
        /// </summary>
        public static LobbyPlayerData FindPlayer(this GameState gameState, string userID)
        {
            return gameState.Team_A.Concat(gameState.Team_B).FirstOrDefault(p => p.userID == userID);
        }
    }
    #endregion
}