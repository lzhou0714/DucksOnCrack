using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
// using Photon.Pun;
using Random = UnityEngine.Random;

public class Generate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstacle;
    public static float maxDim = 150;
    [SerializeField] private int numClusters = 5;
    public GameObject enemies;
    public float radius = 5.0f;
    public float number = 10.0f;

    public static int timer;

    private void Start()
    {
        PlaceObjects();
        PlaceEnemies();
    }

    private void FixedUpdate()
    {
        if (timer < 3) { timer++; }
    }

    void PlaceObjects()
    {
 

        for (int i = 0; i <= number; i++)
        {

            Vector2 position = new Vector2(Random.Range(-maxDim, maxDim), Random.Range(-maxDim, maxDim));
            // if (i == (int)(number / 2.0f))
            // {
            //     obstacle.GetComponent<CircleCollider2D>().radius = 0.5f;
            // }
            // PhotonNetwork.Instantiate(obstacle.name, 
            //     position
            //     ,Quaternion.identity);
            Instantiate(obstacle, position, quaternion.identity);
        }
    }

    void PlaceEnemies()
    {
        for (int j = 0; j < numClusters; j++)
        {
            float genPosX = Random.Range(-maxDim, maxDim);
            float genPosY = Random.Range(-maxDim, maxDim);

            for (int i = 0; i <= number; i++)
            {

                Vector2 position = new Vector2(Random.Range(genPosX, Mathf.Min(genPosX + 10f, maxDim)), Random.Range(genPosY, Mathf.Min(genPosY + 10f, maxDim)));
                // if (i == (int)(number / 2.0f))
                // {
                //     obstacle.GetComponent<CircleCollider2D>().radius = 0.5f;
                // }
                // PhotonNetwork.Instantiate(obstacle.name, 
                //     position
                //     ,Quaternion.identity);
                Instantiate(enemies, position, quaternion.identity);
            }
        }
    }

   

    // private Collider2D[] CheckOverlap(Vector2 position, float radius)
    // {
    //     // Check if there are any colliders within the specified area
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
    //     return colliders;
    // }

}

