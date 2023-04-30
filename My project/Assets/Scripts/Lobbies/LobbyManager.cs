using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomName;
    [SerializeField] GameObject canvas;

    RoomOptions options;

    private void Start()
    {
        options = new RoomOptions();
        options.MaxPlayers = 2;
    }

    public void Join()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void Create()
    {
        PhotonNetwork.CreateRoom(roomName.text, options);
    }

    public void Quickplay()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
