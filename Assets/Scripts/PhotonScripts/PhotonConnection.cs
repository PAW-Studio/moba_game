using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public TMPro.TextMeshProUGUI RoomDetails;
    [SerializeField]
    Button CreateRoomButton, JoinRoomButton;
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        RoomDetails.text = "Connecting..";
        Debug.LogError("Connecting..");
        SetButtonsInteractable(false);
    }
    /// <summary>
    /// Set button interactable true of false
    /// </summary>
    /// <param name="val">Interactable value : true of flase</param>
    private void SetButtonsInteractable(bool val)
    {
        CreateRoomButton.interactable = val;
        JoinRoomButton.interactable = val;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        SetPlayerNickName();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.LogError("Connected..");
        RoomDetails.text = "Connected..";
        SetButtonsInteractable(true);
    }
    public void CreateRoom(string roomName = "") 
    {
        roomName = string.IsNullOrEmpty(roomName) ? ("R_" + Random.Range(99,999)):roomName;
        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(roomName,roomOptions);
    }
    public void JoinRoom() 
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode,string message)
    {
        base.OnJoinRandomFailed(returnCode,message);
        Debug.LogError("Failed to join random room");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {    
        base.OnPlayerEnteredRoom(newPlayer);
       // GameObject playerCharacter= PhotonNetwork.Instantiate("CharacterPrefab",Vector3.zero,Quaternion.identity);
        Debug.LogError("Player " + newPlayer.NickName + "Entered room");
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            //Load scene
            foreach(Player item in PhotonNetwork.PlayerList)
            {
                if(item.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel(1);
                }
            }
        }
    }
    public override void OnJoinedRoom()
    {
        RoomDetails.text = "Room : "+ PhotonNetwork.CurrentRoom.Name+ "\n"+ "Players"+PhotonNetwork.CurrentRoom.PlayerCount+"/"+PhotonNetwork.CurrentRoom.MaxPlayers;
        base.OnJoinedRoom();
        if(PhotonNetwork.CurrentRoom.PlayerCount== PhotonNetwork.CurrentRoom.MaxPlayers) 
        {
            //Load scene
            foreach(Player item in PhotonNetwork.PlayerList)
            {
                if(item.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel(1);
                }
            }
        }
    }

    /// <summary>
    /// Set network player name
    /// </summary>
    public void SetPlayerNickName() 
    {
        string tempName = "Player_" + Random.Range(99,999);
        if(!PlayerPrefs.HasKey("playerName")) 
        {
            PlayerPrefs.SetString("playerName",tempName);
        }
        PhotonNetwork.LocalPlayer.NickName = tempName;
    }

}
