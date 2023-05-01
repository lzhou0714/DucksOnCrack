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
    [SerializeField] TMP_Text errorText;

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
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = message;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
