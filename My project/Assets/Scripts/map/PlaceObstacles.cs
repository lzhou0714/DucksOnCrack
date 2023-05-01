using System;
using UnityEngine;

public class PlaceObstacles : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Generate.timer > 2)
        {
            gameObject.layer = 10; //terrain
            GetComponent<ObstacleBehaviour>().enabled = true;
            GetComponent<CircleCollider2D>().radius = 0.5f;
            Destroy(GetComponent<PlaceObstacles>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("overlap");
        Vector2 pushDirection = transform.position - other.transform.position;
        float pushDistance = 1f; // Adjust the desired push distance

        Generate.timer = 0;
        
        // Move this object away
        transform.position += (Vector3) pushDirection.normalized * pushDistance;
        if (Mathf.Abs(transform.position.y) > Generate.maxDim ||Mathf.Abs(transform.position.x) > Generate.maxDim)
        {
            pushDirection = -pushDirection;
        }

    }
    
}
