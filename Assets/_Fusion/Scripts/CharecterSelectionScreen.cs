using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharecterSelectionScreen : NetworkBehaviour
{
    #region PUBLIC_VAR
    public TMP_Text debugText;
    public GameObject menuScreen;
    public GameObject matchMakingScreen;

    public List<CharecterSelectionData> charecters = new List<CharecterSelectionData>();
    public List<CharacterSelectionButton> characterSelectionButtons = new List<CharacterSelectionButton>();

    public GameObject textPrefab;
    public Transform parentTeamA;
    public Transform parentTeamB;

    public int gameStartTimer = 5;
    public TMP_Text gameStartTimerText;
    public GameObject timerPanel;
    public GameObject uiController;

    public List<RoomPlayer> roomPlayersA = new List<RoomPlayer>();
    public List<RoomPlayer> roomPlayersB = new List<RoomPlayer>();

    public static Action<RoomPlayer> PlayerChanged;
    #endregion

    #region PRIVATE_VAR
    private bool isStartCoroutine = false;
    private static readonly Dictionary<RoomPlayer, LobbyItemUI> ListItems = new Dictionary<RoomPlayer, LobbyItemUI>();
    private static bool IsSubscribed;


    #endregion

    #region UNITY_METHODS
    // Set max player to start game
    // player count reach at max player then game start automaticly 
    // add timer UI for game start 
    // sync timer in all player 
    private void OnEnable()
    {
        if (IsSubscribed) return;

        RoomPlayer.PlayerJoined += AddPlayer;
        RoomPlayer.PlayerLeft += RemovePlayer;
        RoomPlayer.PlayerChanged += EnsureAllPlayersReady;
        RoomPlayer.characterButtonSelect += ChnageButton;
        RoomPlayer.PlayerUITimerChange += ChangeCharacterSelectionTimer;

        IsSubscribed = true;
    }

    private void OnDisable()
    {
        if (!IsSubscribed) return;

        RoomPlayer.PlayerJoined -= AddPlayer;
        RoomPlayer.PlayerLeft -= RemovePlayer;
        RoomPlayer.PlayerChanged -= EnsureAllPlayersReady;
        RoomPlayer.characterButtonSelect -= ChnageButton;
        RoomPlayer.PlayerUITimerChange -= ChangeCharacterSelectionTimer;
        IsSubscribed = false;
    }

    private void AddPlayer(RoomPlayer player)
    {
        /*if (ListItems.ContainsKey(player))
        {
            var toRemove = ListItems[player];
            Destroy(toRemove.gameObject);

            ListItems.Remove(player);
        }*/
        /*Debug.Log("Player Name " + player.Username + " <<----->> " + player.teamName);
        Transform parent = null;
        if (player.teamName == Team.TeamA)
        {
            parent = parentTeamA;
        }
        else
        {
            parent = parentTeamB;
        }
        var obj = Instantiate(textPrefab, parent).GetComponent<LobbyItemUI>();
        obj.SetPlayer(player);

        ListItems.Add(player, obj);*/


        if (player.IsReady)
        {
            characterSelectionButtons[player.KartId].button.interactable = false;
        }
    }

    public void ChnageButton(int id)
    {
        characterSelectionButtons[id].button.interactable = false;
    }
    private void RemovePlayer(RoomPlayer player)
    {
        if (!ListItems.ContainsKey(player))
            return;

        var obj = ListItems[player];
        if (obj != null)
        {
            Destroy(obj.gameObject);
            ListItems.Remove(player);
        }
    }
    #endregion

    #region PUBLIC_METHOD
    public void BackButton()
    {
        gameObject.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void ConfirmButton()
    {
        ReadyUpListener();
        if (RoomPlayer.Local != null)
        {
            characterSelectionButtons[RoomPlayer.Local.KartId].button.interactable = false;
            RoomPlayer.Local.gameObject.GetComponent<PlayerMovement>().SetPlayer(RoomPlayer.Local.KartId);
            RoomPlayer.Local.RPC_SetCharecterButton(RoomPlayer.Local.KartId);
        }

    }

    private void ReadyUpListener()
    {
        var local = RoomPlayer.Local;

        if (local && local.Object && local.Object.IsValid)
        {
            local.RPC_ChangeReadyState(true);
            EnsureAllPlayersReady(local);
        }
    }

    public void CharacterButtonClick(int index)
    {
        var local = RoomPlayer.Local;

        if (local != null && local.IsReady)
        {
            return;
        }

        ClientInfo.KartId = index;

        if (RoomPlayer.Local != null)
        {
            RoomPlayer.Local.RPC_SetKartId(index);
        }

        NetworkManager nm = FindObjectOfType<NetworkManager>();
        nm.charecterIndex = index;
        FusionNetwork.GameManager.Instance.selectedCharacterIndex = index;
        foreach (var item in characterSelectionButtons)
        {
            item.SetSelection(false);
        }
        characterSelectionButtons[index].SetSelection(true);
    }

    private void EnsureAllPlayersReady(RoomPlayer lobbyPlayer)
    {
        if (IsAllReady())
        {
            Debug.Log("All Player Are Ready " + RoomPlayer.Players.Count);
            if (RoomPlayer.Players.Count == FusionNetwork.GameManager.Instance.maxPlayer)
            {
                Debug.Log("All Player is Ready  to join game");
                if (!isStartCoroutine)
                {
                    isStartCoroutine = true;
                    StartCoroutine(GameStartTimer());
                }
            }
            //  int scene = ResourceManager.Instance.tracks[GameManager.Instance.TrackId].buildIndex;
            //LevelManager.LoadTrack(scene);
        }
    }
    private static bool IsAllReady() => RoomPlayer.Players.Count > 0 && RoomPlayer.Players.All(player => player.IsReady);
    public override void Spawned()
    {

    }


    public void CheckAllTeamPlayer()
    {
        foreach (var item in RoomPlayer.Players)
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
        roomPlayersB = bRoom.ToList();

        foreach (var item in roomPlayersA)
        {
            if (ListItems.ContainsKey(item))
            {
                var toRemove = ListItems[item];
                Destroy(toRemove.gameObject);

                ListItems.Remove(item);
            }

            var obj = Instantiate(textPrefab, parentTeamA).GetComponent<LobbyItemUI>();
            obj.SetPlayer(item);

            if (item.currentLane == Lanes.Lane1)
            {
                if (RoomPlayer.Local != null)
                {
                   /* if (RoomPlayer.Local == item)
                        item.RPC_SetTimer(item);*/
                }
              
            }
            ListItems.Add(item, obj);
        }

        foreach (var item in roomPlayersB)
        {

            if (ListItems.ContainsKey(item))
            {
                var toRemove = ListItems[item];
                Destroy(toRemove.gameObject);

                ListItems.Remove(item);
            }

            var obj = Instantiate(textPrefab, parentTeamB).GetComponent<LobbyItemUI>();
            obj.SetPlayer(item);
            ListItems.Add(item, obj);
        }

        if (RoomPlayer.Local != null)
        {
            foreach (var item in ListItems)
            {
                if(item.Key == RoomPlayer.Local)
                {
                    item.Value.StartFirstCharTimer();
                }
            }
        }
    }

    public void ChangeCharacterSelectionTimer(RoomPlayer roomPlayer)
    {
       /* debugText.text = "Connect";
        foreach (var item in roomPlayersA)
        {
            if (!item.isCharacterSelect)
            {
                debugText.text = "In Side " + ListItems[item].playerName;

                ListItems[item].isCharSelecting = true;
                ListItems[item].StartFirstCharTimer();
                break;
            }
        }*/
    }
    #endregion

    #region Coroutine
    IEnumerator GameStartTimer()
    {
        timerPanel.SetActive(true);
        while (gameStartTimer > 0)
        {
            gameStartTimerText.text = "" + gameStartTimer;
            yield return new WaitForSeconds(1);
            gameStartTimer--;
        }
        timerPanel.SetActive(false);
        gameObject.SetActive(false);
        uiController.SetActive(true);
    }
    #endregion


}
