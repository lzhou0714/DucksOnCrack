using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// This controller needs to handle two different cases: One where the player is the driver and one where
    /// the player is the gunner. When the game starts, one instance of the car will be spawned and both players
    /// will be controlling different parts of that car. The components directly attached to each player is solely
    /// their respective UI.
    /// 
    /// With multiplayer, note that for every object with PhotonView, it is linked on the network. Not everything
    /// needs to be linked, especially since linked objects require some extra care in handling the non-local copy
    /// since you typically only want your local copy to receive inputs, display UI, etc.
    /// </summary>

    // PhotonView pv;
    GameManager gm;
    //Gunner gun;
    //Driver drive;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject roleSelect;
    [SerializeField] GameObject gunnerUI;
    [SerializeField] GameObject driverUI;
    [SerializeField] TMP_Text txt;

    void Start()
    {
        // pv = GetComponent<PhotonView>();
        //gun = GetComponent<Gunner>();
        //drive = GetComponent<Driver>();
        gm = FindObjectOfType<GameManager>();

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Disabling role select, not master client");
            roleSelect.SetActive(false);
        }
    }

    private void Update()
    {
        txt.text = "Current players: " + gm.playerCount;
    }

    public void SetRole(int role)
    {
        if (role == 0)  // Driver
        {
            //drive.enabled = true;
            //gun.enabled = false;
            driverUI.SetActive(true);
            gunnerUI.SetActive(false);
        }
        else  // Gunner
        {
            //drive.enabled = false;
            //gun.enabled = true;
            gunnerUI.SetActive(true);
            driverUI.SetActive(false);
        }
    }

    public void SetGMRole(int role)
    {
        gm.SetMasterRole(role);
    }

    public void SetGMStart()
    {
        gm.StartGame();
        roleSelect.SetActive(false);
    }

}
