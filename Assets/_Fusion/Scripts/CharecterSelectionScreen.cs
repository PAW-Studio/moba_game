using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharecterSelectionScreen : NetworkBehaviour
{
    #region PUBLIC_VAR
    public GameObject menuScreen;
    public GameObject matchMakingScreen;

    public List<CharecterSelectionData> charecters = new List<CharecterSelectionData>();
    public List<CharacterSelectionButton> characterSelectionButtons = new List<CharacterSelectionButton>();
    private static readonly Dictionary<RoomPlayer, LobbyItemUI> ListItems = new Dictionary<RoomPlayer, LobbyItemUI>();
    private static bool IsSubscribed;

    public GameObject textPrefab;
    public Transform parentTeamA;
    public Transform parentTeamB;

    public int gameStartTimer = 5;
    public TMP_Text gameStartTimerText;
    public GameObject timerPanel;
    public GameObject uiController;
    public bool isStartCoroutine = false;
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


        IsSubscribed = true;
    }

    private void OnDisable()
    {
        if (!IsSubscribed) return;

        RoomPlayer.PlayerJoined -= AddPlayer;
        RoomPlayer.PlayerLeft -= RemovePlayer;
        RoomPlayer.PlayerChanged -= EnsureAllPlayersReady;


        IsSubscribed = false;
    }

    private void AddPlayer(RoomPlayer player)
    {
        Debug.Log("ITEM " + player.Username);

        if (ListItems.ContainsKey(player))
        {
            var toRemove = ListItems[player];
            Destroy(toRemove.gameObject);

            ListItems.Remove(player);
        }
        Transform parent = null;
        if (player.id % 2 == 0)
        {
            parent = parentTeamA;
        }
        else
        {
            parent = parentTeamB;
        }
        var obj = Instantiate(textPrefab, parent).GetComponent<LobbyItemUI>();
        obj.SetPlayer(player);

        ListItems.Add(player, obj);
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
            RoomPlayer.Local.gameObject.GetComponent<PlayerMovement>().SetPlayer(RoomPlayer.Local.KartId);
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
        ClientInfo.KartId = index;

        if (RoomPlayer.Local != null)
        {
            RoomPlayer.Local.RPC_SetKartId(index);
        }

        NetworkManager.Instance.charecterIndex = index;
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

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetLobbyItemId(int id)
    {

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
