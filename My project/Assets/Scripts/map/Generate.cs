using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Generate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstacle;
    public static float maxDim = 150;
    [SerializeField] private int numClusters = 5;
    public GameObject enemies;
    public float radius = 5.0f;
    public float numObstacles = 100.0f;
    public float enemiesPerCluster = 20f;

    public static int timer;

    private void Start()
    {
        // PlaceObjects();
        // PlaceEnemies();
        PlaceEnemyNormally();
        

    }

    private void FixedUpdate()
    {
        if (timer < 3) { timer++; }
    }

    void PlaceObjects()
    {
        
        for (int i = 0; i <= numObstacles; i++)
        {

            Vector2 position = new Vector2(Random.Range(-maxDim, maxDim), Random.Range(-maxDim, maxDim));
            PhotonNetwork.Instantiate(obstacle.name, position, quaternion.identity);
        }
    }
    

    // void PlaceEnemies()
    // {
    //     for (int j = 0; j < numClusters; j++)
    //     {
    //         float genPosX = Random.Range(-maxDim, maxDim);
    //         float genPosY = Random.Range(-maxDim, maxDim);
    //
    //         for (int i = 0; i <= enemiesPerCluster; i++)
    //         {
    //
    //             Vector2 position = new Vector2(Random.Range(genPosX, Mathf.Min(genPosX + 10f, maxDim)), Random.Range(genPosY, Mathf.Min(genPosY + 10f, maxDim)));
    //             //
    //             // PhotonNetwork.Instantiate(obstacle.name, 
    //             //     position
    //             //     ,Quaternion.identity);
    //             // Instantiate(enemies, position, quaternion.identity);
    //         }
    //     }
    // }

    void PlaceEnemyNormally()
    {
        for (int j = 0; j < numClusters; j++)
        {
            float centreX = Random.Range(-maxDim, maxDim);
            float centreY = Random.Range(-maxDim, maxDim);
            for (int i = 0; i <= enemiesPerCluster; i++)
            {
                float gen1 = Random.Range(0.0f, 1.0f);
                float gen2 = Random.Range(0.0f, 1.0f);
                float r = Mathf.Sqrt(-2.0f * Mathf.Log(gen1));
                float theta = 2.0f * Mathf.PI * gen2;
                double x = r * Math.Cos(theta);
                double y = r * Mathf.Sqrt(theta);
                // Debug.Log($"({gen1},{gen2}");
                Vector2 position = new Vector2((float)x+centreX, (float)y+centreY);
                PhotonNetwork.Instantiate(enemies.name, position, quaternion.identity);

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

