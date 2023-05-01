using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlaceEnemies : MonoBehaviour
{
    private Vector2 pushDirection;
    private int pushCount = 0;
    
    private void FixedUpdate()
    {
        
        // if (Generate.timer > 2)
        // {
        //     gameObject.layer = 6; //enemy hurtbox
        //     GetComponent<CircleCollider2D>().radius = 0.5f;
        //     Destroy(GetComponent<PlaceEnemies>());
        // }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        float maxDim = Generate.maxDim;
        if (pushCount > 50)
        {
            Debug.Log("despawn");
            transform.position = new Vector3(Random.Range(-maxDim, maxDim), Random.Range(-maxDim, maxDim), 0);
            pushCount = 0;
            return;
        }
        Debug.Log("overlap");
        pushDirection = transform.position - other.transform.position;
        float pushDistance = 1f; // Adjust the desired push distance

        Generate.timer = 0;
        
        // Move this object away
        
        // Debug.Log($"push direction: ({pushDirection.normalized},{pushDistance})");
        if (Mathf.Abs(transform.position.y) >maxDim ||Mathf.Abs(transform.position.x) > maxDim)
        {
            pushDirection = -pushDirection;
            
            
        }
        pushCount += 1;
        transform.position += (Vector3) pushDirection.normalized * pushDistance;
        

    }
    
}