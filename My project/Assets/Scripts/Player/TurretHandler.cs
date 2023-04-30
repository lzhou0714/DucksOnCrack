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
            Debug.Log("Not master client, disabling own PhotonView");
            gameObject.SetActive(false);
        }
        if (PhotonNetwork.IsMasterClient && !pv.IsMine)
        {
            Debug.Log("Is master client, disabling other PhotonView");
            gameObject.SetActive(false);
        }
    }

    public void ToParent(int pvid)
    {
        pv.RPC("RPC_ToParent", RpcTarget.All, pvid);
    }

    [PunRPC]
    public void RPC_ToParent(int pvid)
    {
        Debug.Log(pvid);
        Debug.Log(PhotonView.Find(pvid).gameObject.name);
        transform.parent = PhotonView.Find(pvid).gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            transform.Rotate(Vector3.forward, Input.GetAxis("Horizontal"));
        }
    }
}
