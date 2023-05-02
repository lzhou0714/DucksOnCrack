using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyNetworkHandler : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    GameManager man;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)  // Only the master 
        {
            Destroy(this);
        }
        man = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (man.GameStarted() && Time.time % 3 <= Time.deltaTime)
        {
            SpawnEnemy();
        } 
    }

    void SpawnEnemy()
    {
        PhotonNetwork.Instantiate(enemy.name, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), Quaternion.identity);
    }
}
