using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMapTiles : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider2D closestCollider;
    private float closestDistance;

    private void FixedUpdate()
    {
        if (closestCollider != null)
        {
            closestCollider.enabled = false;
            closestCollider = null;
        }
    }

    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        
        if (otherCollider is BoxCollider2D otherBoxCollider)
        {
            // Get the trigger collider involved in the collision
            BoxCollider2D thisCollider = GetComponent<BoxCollider2D>();

            float distance = Vector2.Distance(thisCollider.bounds.center, otherBoxCollider.bounds.center);
            // Debug.Log(distance);
            // Check if the current collider is closer than the previous closest collider
            if (thisCollider.size.x > 1.0f)
                thisCollider.size = new Vector2(1.0f,thisCollider.size.y);
            else if (thisCollider.size.y > 1.0f)
                thisCollider.size = new Vector2(thisCollider.size.x, 1.0f);
            
            if (otherBoxCollider.size.x > 1.0f)
                otherBoxCollider.size = new Vector2(1.0f,otherBoxCollider.size.y);
            else if (otherBoxCollider.size.y > 1.0f)
                otherBoxCollider.size = new Vector2(otherBoxCollider.size.x, 1.0f);
            else if (distance <70)
            {
                thisCollider.enabled = false;
                closestCollider = otherBoxCollider;
                closestDistance = distance;
            }
        }
    }
}
