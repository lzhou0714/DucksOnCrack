using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// The current plan is to use the GameManager to sync all values and handle the correspondences between players.
    /// RPCs will be issued via the GameManager and both players will read from the same networked copy of this. 
    /// </summary>

    [SerializeField] GameObject player;
    PlayerController pc;
    PhotonView pv;

    int masterRole = 0;
    bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        pv = GetComponent<PhotonView>();
        // PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {

        }
    }

    public void StartGame()
    {
        pv.RPC("RPC_SignalStart", RpcTarget.All);
    }

    public void SetMasterRole(int role)
    {
        pv.RPC("RPC_SetMasterRole", RpcTarget.All, role);
    }

    #region RPCs

    [PunRPC]
    public void RPC_SetMasterRole(int role)
    {
        masterRole = role;
        Debug.Log("Master role set");
        if (PhotonNetwork.IsMasterClient)  // Host
        {
            pc.SetRole(masterRole);
        } else  // Client
        {
            pc.SetRole((masterRole + 1) % 2);
        }
    }

    [PunRPC]
    public void RPC_SignalStart()
    {
        gameStarted = true;
        Debug.Log("Game Started! Master role: " + masterRole);
    }

    #endregion
}
