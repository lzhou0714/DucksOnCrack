using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("overlap");
        Vector2 pushDirection = transform.position - other.transform.position;
        float pushDistance = 1f; // Adjust the desired push distance

        // Move this object away
        transform.position += (Vector3) pushDirection.normalized * pushDistance;

    }
}