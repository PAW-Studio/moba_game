using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindMatchScreen : MonoBehaviour
{
    #region PUBLIC_VAR
    public TMP_Text timerText;
    public TMP_Text buttonText;
    public Button findMatchButton;
    public int findGameMaxTime = 60;

    public GameObject characterSelectionScreen;
    #endregion

    #region UNITY_METODS
    private void OnEnable()
    {
        RoomPlayer.PlayerChanged += PlayerCount;
    }

    private void OnDisable()
    {
        RoomPlayer.PlayerChanged -= PlayerCount;
    }
    #endregion

    #region PUBLIC_METHODS

    public void PlayerCount(RoomPlayer lobbyPlayer)
    {
        Debug.Log("RoomPlayer.Players.Count : " + RoomPlayer.Players.Count);
        if (RoomPlayer.Players.Count == FusionNetwork.GameManager.Instance.maxPlayer)
        {
            Debug.Log("Max Player Connected");
        }
    }

    public void FindMatchButtonClick()
    {
        findMatchButton.interactable = false;
        buttonText.text = "IN QUEUE";
        BasicSpawner.Instance.JoinGame();
        StartCoroutine(GameStartTimer());
    }
    #endregion

    #region Coroutine
    IEnumerator GameStartTimer()
    {
        int tempTime = 0;
        while (RoomPlayer.Players.Count != FusionNetwork.GameManager.Instance.maxPlayer)
        {
            timerText.text = "" + tempTime;
            yield return new WaitForSeconds(1);
            tempTime++;
        }
        gameObject.SetActive(false);
        characterSelectionScreen.SetActive(true);
        characterSelectionScreen.transform.LeanScale(Vector3.one, 0.5f);

    }
    #endregion
}
