using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchMakingManager : MonoBehaviourPunCallbacks
{
    public GameObject CharacterSelectionPanel;
    public static MatchMakingManager Instance;
    public bool championSelected;
    private void Awake()
    {
        if(Instance == null) Instance = this;
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.PlayerList.Length >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
           // ShowChampionSelectionPanel();
        }
    }

    private void ShowChampionSelectionPanel()
    {
        photonView.RPC("ShowCharacterSelectScreen",RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        foreach(var player in PhotonNetwork.PlayerList)
        {
            if(PhotonNetwork.PlayerList.Length >= PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                if(player.IsMasterClient)
                {// ShowChampionSelectionPanel();
                }
            }
        }
        
    }
   
    [PunRPC]
    public void ShowCharacterSelectScreen() 
    {
        CharacterSelectionPanel.SetActive(true);
    }

    public void ConfirmChampion() 
    {

    }

}
