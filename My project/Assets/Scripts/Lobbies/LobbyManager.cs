using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomName;
    [SerializeField] GameObject canvas;

    private void Start()
    {
    }

    public void Join()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void Create()
    {
        PhotonNetwork.CreateRoom(roomName.text);
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
