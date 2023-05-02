using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7 && PhotonNetwork.IsMasterClient)
        {
            col.GetComponent<HPEntity>().TakeDamage(10);
        }
    }
}
