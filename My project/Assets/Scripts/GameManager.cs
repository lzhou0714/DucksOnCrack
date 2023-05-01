using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// The current plan is to use the GameManager to sync all values and handle the correspondences between players.
    /// RPCs will be issued via the GameManager and both players will read from the same networked copy of this. 
    /// </summary>

    [SerializeField] GameObject car;
    [SerializeField] GameObject gun;
    [SerializeField] PhotonView vehiclePv;
    [SerializeField] PhotonView gunPv;
    PlayerController pc;
    PhotonView pv;
    GameObject truck;
    GameObject ak;
    int pvid;

    Player client;
    Player host;
    int playerCount = 0;

    int masterRole = 0;
    bool gameStarted = false;


    void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        pv = GetComponent<PhotonView>();
        truck = PhotonNetwork.Instantiate(car.name, Vector3.zero, Quaternion.identity);
        ak = PhotonNetwork.Instantiate(gun.name, Vector3.zero, Quaternion.identity);

        vehiclePv = truck.GetComponent<PhotonView>();
        gunPv = ak.GetComponent<PhotonView>();

        pvid = vehiclePv.ViewID;
        Debug.Log(pvid);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {

        }
    }

    public bool GameStarted()
    {
        return gameStarted;
    }

    public void StartGame()
    {
        pv.RPC("RPC_SignalStart", RpcTarget.All);
    }

    public void SetMasterRole(int role)
    {
        pv.RPC("RPC_SetMasterRole", RpcTarget.All, role);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (newPlayer.IsMasterClient)
        {
            host = newPlayer;
        } else
        {
            client = newPlayer;
        }
        ++playerCount;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        --playerCount;
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
        if (PhotonNetwork.IsMasterClient)
        {
            // Assign ownership of parts of the car to the other player
            if (masterRole == 0)  // Host is driver
            {
                Debug.Log("Transferring gun");
                gunPv.TransferOwnership(client);
                vehiclePv.TransferOwnership(host);
            } 
            else
            {
                vehiclePv.TransferOwnership(client);
                gunPv.TransferOwnership(host);
                Debug.Log("Transferring car");
            }
        }
        ak.GetComponent<TurretHandler>().ToParent(pvid);
        Debug.Log("Game Started! Master role: " + masterRole);
    }

    #endregion
}
