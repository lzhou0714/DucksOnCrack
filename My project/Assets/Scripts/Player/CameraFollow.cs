using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;
    public Vector3 offset;
    [SerializeField] public float trackingRate = 0.1f;

    bool targetAssigned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AssignTarget(Transform trfm)
    {
        target = trfm;
        offset = transform.position - target.position;
        targetAssigned = true;
    }

    // Late update is called once per frame, after Update and FixedUpdate, good for smooth following
    void LateUpdate()
    {
        if (targetAssigned)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, trackingRate);
            transform.position = newPosition;
            transform.LookAt(target);
        }
    }
}
