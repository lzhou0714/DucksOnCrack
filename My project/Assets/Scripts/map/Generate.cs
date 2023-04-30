using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Generate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstacle;
    public float radius = 5.0f;
    public float number = 10.0f;

    private void Start()
    {
        PlaceObjects();

    }

    void PlaceObjects()
    {
        for (int i = 0; i <= number; i++)
        {
            Vector2 position = new Vector2(Random.Range(-5.0f, 5.0f),  Random.Range(-5.0f, 5.0f));
            PhotonEditor.Instantiate(obstacle, 
                position
                ,Quaternion.identity);
        }
    }

   

    // private Collider2D[] CheckOverlap(Vector2 position, float radius)
    // {
    //     // Check if there are any colliders within the specified area
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
    //     return colliders;
    // }

}

