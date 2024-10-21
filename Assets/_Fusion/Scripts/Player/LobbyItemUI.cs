using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItemUI : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text choosingCharText;
    public Image icon;
    private RoomPlayer _player;
    public CharecterDataScriptable charecterDataScriptable;

    public void SetPlayer(RoomPlayer player)
    {
        _player = player;
        playerName.text = player.Username.Value;

      
    }

    private void Update()
    {
        if (_player.Object != null && _player.Object.IsValid)
        {
            playerName.text = _player.Username.Value;

            icon.sprite = charecterDataScriptable.GetCharecterSprite(_player.KartId);
            if (_player.IsReady)
                choosingCharText.text = "Ready";
            else
                choosingCharText.text = "Choosing...";
        }

    }
}

