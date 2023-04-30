using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [SerializeField] public float trackingRate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Late update is called once per frame, after Update and FixedUpdate, good for smooth following
    void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, trackingRate);
        transform.position = newPosition;
        transform.LookAt(target);
    }
}
