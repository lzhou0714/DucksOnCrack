using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VehicleHandler : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime;
        }
    }
}
