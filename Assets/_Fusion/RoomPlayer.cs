using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class RoomPlayer : NetworkBehaviour
{
    public enum EGameState
    {
        Lobby,
        GameCutscene,
        GameReady
    }

    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;
    public static Action<RoomPlayer> PlayerUITimerChange;

    public static Action<int> characterButtonSelect;

    public static RoomPlayer Local;

    [Networked] public NetworkBool IsReady { get; set; }
    [Networked] public NetworkString<_32> Username { get; set; }

    [Networked] public NetworkBool HasFinished { get; set; }
    //	[Networked] public KartController Kart { get; set; }
    [Networked] public EGameState GameState { get; set; }
    [Networked] public int KartId { get; set; }
    [Networked] public int id { get; set; }
    [Networked] public int charSelectionTimer { get; set; }

    [Networked] public Team teamName { get; set; }

    [Networked] public NetworkBool IsFindMatch { get; set; }
    [Networked] public NetworkBool isCharacterSelect { get; set; }

    [Networked] public Lanes currentLane { get; set; }
    public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;

    private ChangeDetector _changeDetector;

    public PlayerMovement playerMovement;

    public List<LaneInfo> laneInfos = new List<LaneInfo>();

    public bool isTimerCoroutineStart = false;



    public override void Spawned()
    {
        base.Spawned();

        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (Object.HasInputAuthority)
        {
            Local = this;
            PlayerChanged?.Invoke(this);
            ClientInfo.Username = "Player - " + UnityEngine.Random.Range(1, 1000);
            RPC_SetPlayerStats(ClientInfo.Username, ClientInfo.KartId);

        }
        Players.Add(this);
        PlayerJoined?.Invoke(this);
        DontDestroyOnLoad(gameObject);




        /*  foreach (var item in Players)
          {
              if (item.teamName == Team.TeamA)
              {
                  if (!roomPlayersA.Contains(item))
                      roomPlayersA.Add(item);
              }
              else
              {
                  if (!roomPlayersB.Contains(item))
                      roomPlayersB.Add(item);
              }
          }

          var aRoom = roomPlayersA.OrderBy(x => x.currentLane);
          roomPlayersA = aRoom.ToList();

          var bRoom = roomPlayersB.OrderBy(x => x.currentLane);
          roomPlayersB = bRoom.ToList();*/
        /* foreach (var item in Players)
         {
             Debug.Log("ID : " + item.id + " NAME : " + item.Username + "Team : " + item.teamName);
             if(item.teamName == Team.TeamA)
             {
                 roomPlayers.Add(item);
             }
         }*/
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(IsReady):
                case nameof(Username):
                    OnStateChanged(this);
                    break;
            }
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RPC_SetPlayerStats(NetworkString<_32> username, int kartId)
    {
        Username = username;
        KartId = kartId;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetKartId(int id)
    {
        playerMovement.SetPlayer(id);
        KartId = id;
    }

    //[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
   /* public void RPC_SetTimer(RoomPlayer room)
    {
        if (!room.isTimerCoroutineStart)
        {
            room.isTimerCoroutineStart = true;
            StartCoroutine(TimerCoroutine(room));
        }
    }*/

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetTeam(Team team)
    {
        teamName = team;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetCharecterButton(int id)
    {
        Debug.Log("Character Button index " + id);
        characterButtonSelect?.Invoke(id);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetId(int _id)
    {
        id = _id;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_ChangeReadyState(NetworkBool state)
    {
        Debug.Log($"Setting {Object.Name} ready state to {state}");
        IsReady = state;
    }

    private void OnDisable()
    {
        // OnDestroy does not get called for pooled objects
        PlayerLeft?.Invoke(this);
        Players.Remove(this);
    }

    public static void OnStateChanged(RoomPlayer changed) => PlayerChanged?.Invoke(changed);

    public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
    {
        var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == p);
        if (roomPlayer != null)
        {
            Players.Remove(roomPlayer);
            runner.Despawn(roomPlayer.Object);
        }
    }

    public void NextPlayerTimer()
    {
        foreach (var item in Players)
        {
            if (!item.isCharacterSelect)
            {
              //  item.RPC_SetTimer(item);
                break;
            }
        }

        /*
        foreach (var item in Players)
        {
            if (item.teamName == Team.TeamA)
            {
                if (!roomPlayersA.Contains(item))
                    roomPlayersA.Add(item);
                else
                {
                    
                }
            }
            else
            {
                if (!roomPlayersB.Contains(item))
                    roomPlayersB.Add(item);
            }
        }

        var aRoom = roomPlayersA.OrderBy(x => x.currentLane);
        roomPlayersA = aRoom.ToList();

        var bRoom = roomPlayersB.OrderBy(x => x.currentLane);
        roomPlayersB = bRoom.ToList();

        foreach (var item in roomPlayersA)
        {
            if (!item.isCharacterSelect)
            {
                item.RPC_SetTimer(item);
                break;
            }
        }
        foreach (var item in Players)
        {
            if (item.isCharacterSelect)
                Debug.Log("IS DONE");
            else
                Debug.Log("IS NOT DONE");

        }*/

        /* bool teamASelectionDone = false;
         foreach (var item in roomPlayersA)
         {
             if (item.isCharacterSelect)
                 teamASelectionDone = true;
             else
                 teamASelectionDone = false;
         }
         if (teamASelectionDone)
         {
             foreach (var item in roomPlayersB)
             {
                 if (!item.isCharacterSelect)
                 {
                     item.RPC_SetTimer(item);
                     break;
                 }
             }
         }*/
    }
   

   
    #region COROUTINE

   public IEnumerator TimerCoroutine()
    {
        while (charSelectionTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            charSelectionTimer--;
            Debug.Log("Timer " + charSelectionTimer);
        }
        Debug.Log("Timer Complete");
        isCharacterSelect = true;
     
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Configure(string name)
    {
        Debug.LogError("WOHOOOOOOO!! => " + name);
        if (Players.Count == FusionNetwork.GameManager.Instance.maxPlayer)
        {
            Debug.LogError("ALL player connected");
        }
    }

    public void Call_RPC()
    {
        RPC_Configure("Hey there!!!");
    }



   
    #endregion
}