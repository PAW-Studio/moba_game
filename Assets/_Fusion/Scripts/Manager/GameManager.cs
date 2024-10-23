namespace FusionNetwork
{
    using Fusion;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class GameManager : NetworkBehaviour
    {
        #region PUBLIC_VAR
        public CameraFollow cameraFollow;
        public int selectedCharacterIndex = 0;
        public int maxPlayer = 2;

        public static GameManager Instance;
        #endregion

        private GameState gameState;

        public Dictionary<string, ServerPlayerData> players = new Dictionary<string, ServerPlayerData>();

        private void Awake()
        {
            Instance = this;
        }

        public void StartTeamAllocation()
        {
            // Team allocation
            StartCoroutine(AllocateTeams());
        }

        #region LOGIC

        private IEnumerator AllocateTeams()
        {
            gameState = new GameState();

            int playerCounter = 0;

            Debug.LogError("AllocateTeams");
            gameState.state = GameStateEnum.LaneAllocation;
            foreach (var playerData in players.Values)
            {
                LobbyPlayerData lobbyPlayer = new LobbyPlayerData
                {
                    userID = playerData.userID,
                    playerName = playerData.playerName,
                    laneIndex = playerCounter % 5, // Assign laneIndex 0 to 4
                    selectedCharacter = -1, // You can customize this as per player choice
                    individualTimer = 0f // Initialize or set default timer if needed
                };

                if (playerCounter % 2 == 0)
                {
                    gameState.Team_A.Add(lobbyPlayer); // Assign to Team A
                }
                else
                {
                    gameState.Team_B.Add(lobbyPlayer); // Assign to Team B
                }

                playerCounter++;
            }

            RPC_SendGameState(JsonUtility.ToJson(gameState));

            yield return new WaitForSeconds(5f);

            gameState.state = GameStateEnum.TeamSelection_A;
            StartCoroutine(HandleTeamSelection(gameState, gameState.Team_A, 0)); // Move to Team A
        }

        Coroutine commonRoundTimerRoutine = null;
        Coroutine individualTimerRoutine = null;
        private IEnumerator CommonRoundTimer(GameState gameState, int teamIndex)
        {
            gameState.commonTimer = 20f; // Players per team * 10
            RPC_SendGameState(JsonUtility.ToJson(gameState));

            while (gameState.commonTimer > 0)
            {
                yield return new WaitForSeconds(1f);
                gameState.commonTimer -= 1f;
                RPC_SendGameState(JsonUtility.ToJson(gameState));

                //// Check if all players in the team have selected a character
                //if (teamIndex == 0 && gameState.Team_A.All(p => p.selectedCharacter != -1))
                //{
                //    break; // Stop the common timer for Team A when all players have selected their character
                //}
                //else if (teamIndex == 1 && gameState.Team_B.All(p => p.selectedCharacter != -1))
                //{
                //    break; // Stop the common timer for Team B when all players have selected their character
                //}

                Debug.Log($"Common Timer: {gameState.commonTimer}");
            }

            if (gameState.commonTimer <= 0)
            {
                Debug.Log("Common timer ran out!");
            }

            //// Transition to the next team if Team A has finished
            //if (teamIndex == 0)
            //{
            //    gameState.state = GameStateEnum.TeamSelection_B;
            //    StartCoroutine(HandleTeamSelection(gameState, gameState.Team_B, 1)); // Move to Team B
            //}
            //else
            //{
            //    Debug.LogError("Game will start!");
            //}
        }

        private IEnumerator IndividualRoundTimer(LobbyPlayerData player, GameState gameState, List<LobbyPlayerData> team, int currentPlayerIndex)
        {
            player.individualTimer = 10f;
            RPC_SendGameState(JsonUtility.ToJson(gameState));

            while (player.individualTimer > 0)
            {
                yield return new WaitForSeconds(1f);
                player.individualTimer -= 1f;
                RPC_SendGameState(JsonUtility.ToJson(gameState));
                Debug.Log($"Player {player.playerName}'s Individual Timer: {player.individualTimer}");

                // If player has already selected their character, stop the timer
                if (player.selectedCharacter != -1)
                {
                    Debug.Log($"{player.playerName} selected a character.");
                    break;
                }
            }

            // Move to the next player if their time runs out or they select a character
            if (player.individualTimer <= 0)
            {
                Debug.Log($"Player {player.playerName}'s timer ran out!");
            }

            // Move to the next player
            currentPlayerIndex++;
            if (currentPlayerIndex < team.Count)
            {
                StartCoroutine(IndividualRoundTimer(team[currentPlayerIndex], gameState, team, currentPlayerIndex));
            }
            else
            {
                // If all players in Team A have selected, move on to Team B or end the round
                if (gameState.state == GameStateEnum.TeamSelection_A)
                {
                    Debug.LogError("Team A timer runs out!");
                    StopCoroutine(commonRoundTimerRoutine); // Stop Team A's common timer

                    gameState.state = GameStateEnum.TeamSelection_B;
                    StartCoroutine(HandleTeamSelection(gameState, gameState.Team_B, 1)); // Move to Team B
                }
                else if (gameState.state == GameStateEnum.TeamSelection_B)
                {
                    Debug.LogError("Team B timer runs out!");
                    StopCoroutine(commonRoundTimerRoutine); // Stop Team B's common timer

                    Debug.LogError("Starting the game!!");
                }
            }
        }

        private IEnumerator HandleTeamSelection(GameState gameState, List<LobbyPlayerData> team, int teamIndex)
        {
            // Start common timer for the team
            commonRoundTimerRoutine = StartCoroutine(CommonRoundTimer(gameState, teamIndex));

            // Start individual timers for each player in the team, starting with the first player
            yield return StartCoroutine(IndividualRoundTimer(team[0], gameState, team, 0));
        }

        public void SelectCharacter()
        {
            RPC_SelectCharacter("123", 1);
        }

        [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
        public void RPC_SelectCharacter(string userID, int characterIndex)
        {
            Debug.LogError("Selected the character!");
            NetworkManager nm = FindObjectOfType<NetworkManager>();
            if (nm._networkRunner.IsServer)
            {
                foreach (var player in gameState.Team_A.Concat(gameState.Team_B))
                {
                    if (player.userID == userID)
                    {
                        player.selectedCharacter = characterIndex;
                        Debug.Log($"Player {player.playerName} selected character {characterIndex}");
                        break;
                    }
                }
            }
        }


        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        public void RPC_SendGameState(string jsonData)
        {
            GameState gameState = JsonUtility.FromJson<GameState>(jsonData);

            NetworkManager nm = FindObjectOfType<NetworkManager>();
            if (!nm._networkRunner.IsServer)
            {
                if (gameState.state == GameStateEnum.LaneAllocation)
                {
                    PrintGameState(gameState);
                    //Debug.Log("Showing lanes!!");
                }
                else if (gameState.state == GameStateEnum.TeamSelection_A)
                {
                    PrintGameState(gameState);
                    //Debug.Log("Team A is selecting characters!!");
                }
                else if (gameState.state == GameStateEnum.TeamSelection_B)
                {
                    PrintGameState(gameState);
                    //Debug.Log("Team B is selecting characters!!");
                }
            }
        }

        private void PrintGameState(GameState gameState)
        {
            // Get current date and time for the log entry
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Create a StringBuilder to build the log message
            System.Text.StringBuilder logMessage = new System.Text.StringBuilder();

            // Add the header with timestamp
            logMessage.AppendLine($"[{timestamp}] === GameState ===");

            // Add common timer and team selection state
            logMessage.AppendLine($"[{timestamp}] Common Timer: {gameState.commonTimer}");
            logMessage.AppendLine($"[{timestamp}] Is First Team Selecting: {gameState.isFirstTeamSelecting}");

            // Add details of Team A
            logMessage.AppendLine($"[{timestamp}] === Team A ===");
            foreach (var player in gameState.Team_A)
            {
                logMessage.AppendLine(
                    $"[{timestamp}] Player Name: {player.playerName}, " +
                    $"UserID: {player.userID}, " +
                    $"Lane Index: {player.laneIndex}, " +
                    $"Selected Character: {player.selectedCharacter}, " +
                    $"Individual Timer: {player.individualTimer}"
                );
            }

            // Add details of Team B
            logMessage.AppendLine($"[{timestamp}] === Team B ===");
            foreach (var player in gameState.Team_B)
            {
                logMessage.AppendLine(
                    $"[{timestamp}] Player Name: {player.playerName}, " +
                    $"UserID: {player.userID}, " +
                    $"Lane Index: {player.laneIndex}, " +
                    $"Selected Character: {player.selectedCharacter}, " +
                    $"Individual Timer: {player.individualTimer}"
                );
            }

            // Add footer
            logMessage.AppendLine($"[{timestamp}] === End of GameState ===");

            // Print the entire log message as a single formatted string
            Debug.Log(logMessage.ToString());
        }
        #endregion
    }
}