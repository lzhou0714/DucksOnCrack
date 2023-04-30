using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretHandler : MonoBehaviour
{
    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        // Disable the non host copy of the vehicle
        if (!PhotonNetwork.IsMasterClient && pv.IsMine)
        {
            gameObject.SetActive(false);
        }
        if (PhotonNetwork.IsMasterClient && !pv.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            
        }
    }
}
