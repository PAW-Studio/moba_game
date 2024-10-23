using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class PlayerSelectionManager : NetworkBehaviour
{
    public float selectionTime = 30f;  // 30 seconds for selection
    public TMP_Text timerText;  // Reference to the UI Text component to display the timer
    public int currentPlayerIndex = 0;  // Track current player
    public bool selectionStarted = false;
    public NetworkRunner _unner;
    [Networked] // Sync the timer across all clients
    public TickTimer timer { get; set; }

    public bool isSpawn = false;
    public override void Spawned()
    {
        isSpawn = true;
        base.Spawned();
    }
    void Start()
    {


        /*  if (Runner.IsServer)
          {
              StartPlayerSelection();
          }*/
    }

    void Update()
    {

        if (!isSpawn)
            return;
        timerText.text = $"Time Remaining: {timer} seconds";

        if (!selectionStarted) return;

        // Update the UI with the remaining time
        UpdateTimerUI();

        // Check if time runs out
        if (timer.Expired(Runner))
        {
            NextPlayerSelection();
        }
    }

    public void StartPlayerSelection(NetworkRunner networkRunner)
    {
        timer = TickTimer.CreateFromSeconds(networkRunner, selectionTime);

        _unner = networkRunner;
        selectionStarted = true;
    }

    private void NextPlayerSelection()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= Runner.ActivePlayers.Count())
        {
            // All players have selected
            selectionStarted = false;
            EndSelectionPhase();
            return;
        }

        // Reset timer for next player
        StartPlayerSelection(Runner);
    }

    private void EndSelectionPhase()
    {
        // Handle what happens after all players have selected characters
        Debug.Log("Selection phase ended.");
        timerText.text = "Selection phase ended";
    }

    // Update the UI Text to show the remaining time
    private void UpdateTimerUI()
    {
        float remainingTime = timer.RemainingTime(Runner) ?? 0f;
        timerText.text = $"Time Remaining: {remainingTime:F1} seconds";
      //  Debug.Log("------------------- >>>> " + remainingTime);
    }
}
